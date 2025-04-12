using saga.Infrastructure.Repositories;
using saga.Models.DTOs;
using saga.Models.Mapper;
using saga.Services.Interfaces;

namespace saga.Services
{
    public class ProfessorService : IProfessorService
    {
        private readonly IRepository _repository;
        private readonly ILogger<ProfessorService> _logger;
        private readonly IUserService _userService;

        public ProfessorService(
            IRepository repository,
            ILogger<ProfessorService> logger,
            IUserService userService
        )
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        /// <inheritdoc />
        public async Task<ProfessorInfoDto> CreateProfessorAsync(ProfessorDto professorDto)
        {
            var user = await _userService.CreateUserAsync(professorDto);
            var professor = professorDto.ToEntity(user.Id);
            professor = await _repository.Professor.AddAsync(professor);
            if (professorDto.ProjectIds.Any())
                await _repository.ProfessorProject.HandleByProfessor(professorDto.ProjectIds.Select(Guid.Parse), professor);

            _logger.LogInformation($"Professor {professor.User.Id} created successfully.");
            return professor.ToDto();
        }

        /// <inheritdoc />
        public async Task<ProfessorInfoDto> GetProfessorAsync(Guid id)
        {
            var professor = await _repository
                .Professor
                .GetByIdAsync(id, x => x.User) ?? throw new ArgumentException("Professor not found.");
            return professor.ToDto();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ProfessorInfoDto>> GetAllProfessorsAsync()
        {
            var professors = await _repository.Professor.GetAllAsync(x => x.User);
            var professorDtos = new List<ProfessorInfoDto>();
            foreach (var professor in professors)
            {
                professorDtos.Add(professor.ToDto());
            }

            return professorDtos;
        }

        /// <inheritdoc />
        public async Task<ProfessorInfoDto> UpdateProfessorAsync(Guid id, ProfessorDto professorDto)
        {
            var existingProfessor = await _repository.Professor.GetByIdAsync(id, x => x.User);
            if (existingProfessor == null)
            {
                throw new ArgumentException($"Professor with id {id} does not exist.");
            }

            existingProfessor = professorDto.ToEntity(existingProfessor);
            await _repository.Professor.UpdateAsync(existingProfessor);
            await _repository.ProfessorProject.HandleByProfessor(professorDto.ProjectIds.Select(Guid.Parse), existingProfessor);

            return existingProfessor.ToDto();
        }

        /// <inheritdoc />
        public async Task DeleteProfessorAsync(Guid id)
        {
            var existingProfessor = await _repository.Professor.GetByIdAsync(id);
            if (existingProfessor == null)
            {
                throw new ArgumentException($"Professor with id {id} does not exist.");
            }

            await _repository.Professor.DeactiveAsync(existingProfessor);
        }
    }
}
