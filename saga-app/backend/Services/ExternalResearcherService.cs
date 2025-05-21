using saga.Infrastructure.Repositories;
using saga.Models.DTOs;
using saga.Models.Mapper;
using saga.Services.Interfaces;

namespace saga.Services
{
    public class ExternalResearcherService : IExternalResearcherService
    {
        private readonly IRepository _repository;
        private readonly ILogger<ExternalResearcherService> _logger;
        private readonly IUserService _userService;

        public ExternalResearcherService(
            IRepository repository,
            ILogger<ExternalResearcherService> logger,
            IUserService userService
        )
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        /// <inheritdoc />
        public async Task<ExternalResearcherDto> CreateExternalResearcherAsync(ExternalResearcherDto externalResearcherDto)
        {
            var userId = (await _userService.CreateUserAsync(externalResearcherDto)).Id;
            var externalResearcher = externalResearcherDto.ToEntity(userId);
            externalResearcher = await _repository.ExternalResearcher.AddAsync(externalResearcher);

            _logger.LogInformation($"ExternalResearcher {externalResearcher.User.Id} created successfully.");
            return externalResearcher.ToDto();
        }

        /// <inheritdoc />
        public async Task<ExternalResearcherDto> GetExternalResearcherAsync(Guid id)
        {
            var externalResearcherEntity = await _repository.ExternalResearcher.GetByIdAsync(id, x => x.User);
            if (externalResearcherEntity == null)
            {
                throw new ArgumentException("ExternalResearcher not found.");
            }

            return externalResearcherEntity.ToDto();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ExternalResearcherDto>> GetAllExternalResearchersAsync()
        {
            var externalResearchers = await _repository.ExternalResearcher.GetAllAsync(x => x.User);
            var externalResearcherDtos = new List<ExternalResearcherDto>();
            foreach (var externalResearcher in externalResearchers)
            {
                externalResearcherDtos.Add(externalResearcher.ToDto());
            }

            return externalResearcherDtos;
        }

        /// <inheritdoc />
        public async Task<ExternalResearcherDto> UpdateExternalResearcherAsync(Guid id, ExternalResearcherDto externalResearcherDto)
        {
            var existingExternalResearcher = await _repository.ExternalResearcher.GetByIdAsync(id);
            if (existingExternalResearcher == null)
            {
                throw new ArgumentException($"ExternalResearcher with id {id} does not exist.");
            }

            existingExternalResearcher = externalResearcherDto.ToEntity(existingExternalResearcher);

            await _repository.ExternalResearcher.UpdateAsync(existingExternalResearcher);

            return existingExternalResearcher.ToDto();
        }

        /// <inheritdoc />
        public async Task DeleteExternalResearcherAsync(Guid id)
        {
            var existingExternalResearcher = await _repository.ExternalResearcher.GetByIdAsync(id);
            if (existingExternalResearcher == null)
            {
                throw new ArgumentException($"ExternalResearcher with id {id} does not exist.");
            }

            await _repository.ExternalResearcher.DeactiveAsync(existingExternalResearcher);
        }
    }
}
