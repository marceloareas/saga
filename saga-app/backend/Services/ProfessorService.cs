using saga.Infrastructure.Repositories;
using saga.Models.DTOs;
using saga.Models.Mapper;
using saga.Services.Interfaces;
using saga.Models.DTOs.Common;

namespace saga.Services
{
    public class ProfessorService : IProfessorService
    {
        private readonly IRepository _repository;
        private readonly ILogger<ProfessorService> _logger;
        private readonly IUserService _userService;
        private readonly Validations _validations;


        public ProfessorService(
            IRepository repository,
            ILogger<ProfessorService> logger,
            IUserService userService,
            Validations validations
        )
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _validations = validations ?? throw new ArgumentNullException(nameof(validations));
        }

        /// <inheritdoc />
        public async Task<ProfessorInfoDto> CreateProfessorAsync(ProfessorDto professorDto)
        {
            (var canCreate, var msgCreate) = await _validations.ProfessorValidator.CanCreate(professorDto);
            if (!canCreate) throw new ArgumentException(msgCreate);

            ProfessorEntity? professor = null;
            await _repository.ExecuteInTransactionAsync(async () =>
            {
                var user = await _userService.CreateUserAsync(professorDto);
                professor = await _repository.Professor.AddAsync(professorDto.ToEntity(user.Id));
                var desired = (professorDto.ProjectIds ?? Enumerable.Empty<string>()).Select(Guid.Parse);
                await _repository.ProfessorProject.HandleByProfessor(desired, professor!);
                await _repository.CommitAsync();
            });

             _logger.LogInformation("Professor {UserId} created successfully.", professor!.User.Id);
            return professor!.ToDto();
        }

        /// <inheritdoc />
        public async Task<ProfessorInfoDto> GetProfessorAsync(Guid id)
        {
            var professor = await _repository
                .Professor
                .GetByIdAsync(id, x => x.User) ?? throw new NotFoundException($"Professor with id {id} not found.");
            return professor.ToDto();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ProfessorInfoDto>> GetAllProfessorsAsync()
        {
            var professors = await _repository.Professor.GetAllUnfilteredAsync(x => x.User);
            var professorDtos = new List<ProfessorInfoDto>();
            foreach (var professor in professors)
            {
                professorDtos.Add(professor.ToDto());
            }

            return professorDtos;
        }

                /// <inheritdoc />
        public async Task<IEnumerable<ProfessorInfoDto>> GetAllProfessorsUnfilteredAsync()
        {
            var professors = await _repository.Professor.GetAllUnfilteredAsync(x => x.User);
            var professorDtos = new List<ProfessorInfoDto>();
            foreach (var professor in professors)
            {
                professorDtos.Add(professor.ToDto());
            }

            return professorDtos;
        }

        public async Task<PagedResult<ProfessorInfoDto>> GetProfessorsPageAsync(int page = 1, int pageSize = 50, string? q = null)
        {
            var (items, total) = await _repository.Professor.GetPagedAsync(page, pageSize, q, x => x.User);
            return new PagedResult<ProfessorInfoDto>
            {
                Items = items.Select(p => p.ToDto()).ToList(),
                Page = page,
                PageSize = pageSize,
                TotalCount = total
            };
        }

        /// <inheritdoc />
        public async Task<ProfessorInfoDto> UpdateProfessorAsync(Guid id, ProfessorDto professorDto)
        {
            var existingProfessor = await _repository.Professor.GetByIdAsync(id);
            if (existingProfessor == null)
                throw new NotFoundException($"Professor with id {id} not found.");

            (var canUpdate, var msgUpdate) = await _validations.ProfessorValidator.CanUpdate(professorDto, id);
            if (!canUpdate) throw new ArgumentException(msgUpdate);

            existingProfessor = professorDto.ToEntity(existingProfessor);
            
           await _repository.ExecuteInTransactionAsync(async () =>
            {
                await _userService.UpdateUserAsync(existingProfessor.UserId, professorDto);
                await _repository.Professor.UpdateAsync(existingProfessor);
                var desired = (professorDto.ProjectIds ?? Enumerable.Empty<string>()).Select(Guid.Parse);
                await _repository.ProfessorProject.HandleByProfessor(desired, existingProfessor);
                await _repository.CommitAsync();
            });

            return existingProfessor.ToDto();
        }

        /// <inheritdoc />
        public async Task DeleteProfessorAsync(Guid id)
        {
            var existingProfessor = await _repository.Professor.GetByIdAsync(id);
            if (existingProfessor == null)
                throw new NotFoundException($"Professor with id {id} not found.");

            await _repository.ExecuteInTransactionAsync(async () =>
            {
                await _repository.Professor.DeactiveAsync(existingProfessor);
                await _userService.DeleteUserAsync(existingProfessor.UserId);
                await _repository.CommitAsync();
            });
        }
    }
}
