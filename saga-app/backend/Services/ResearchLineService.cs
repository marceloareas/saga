using saga.Infrastructure.Extensions;
using saga.Infrastructure.Repositories;
using saga.Models.DTOs;
using saga.Models.Mapper;
using saga.Services.Interfaces;

namespace saga.Services
{
    public class ResearchLineService : IResearchLineService
    {
        private readonly IRepository _repository;
        private readonly ILogger<ResearchLineService> _logger;

        public ResearchLineService(
            IRepository repository,
            ILogger<ResearchLineService> logger
        )
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public async Task<ResearchLineInfoDto> CreateResearchLineAsync(ResearchLineDto researchLineDto)
        {
            try
            {
                var researchLine = researchLineDto.ToEntity();

                researchLine = await _repository.ResearchLine.AddAsync(researchLine);

                _logger.LogInformation($"ResearchLine {researchLine.Name} created successfully.");
                return researchLine.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"ResearchLine {researchLineDto.Name} as {ex}");
                return researchLineDto.ToEntity().ToDto();
            };
        }

        /// <inheritdoc />
        public async Task<ResearchLineInfoDto> GetResearchLineAsync(Guid id)
        {
            var researchLineEntity = await _repository
                .ResearchLine
                .GetByIdAsync(id, x => x.Projects);
            if (researchLineEntity == null)
            {
                throw new ArgumentException("ResearchLine not found.");
            }

            return researchLineEntity.ToDto();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ResearchLineInfoDto>> GetAllResearchLinesAsync()
        {
            var researchLines = await _repository
                .ResearchLine
                .GetAllAsync(x => x.Projects);
            var researchLineDtos = new List<ResearchLineInfoDto>();
            foreach (var researchLine in researchLines)
            {
                researchLineDtos.Add(researchLine.ToDto());
            }

            return researchLineDtos;
        }

        /// <inheritdoc />
        public async Task<ResearchLineInfoDto> UpdateResearchLineAsync(Guid id, ResearchLineDto researchLineDto)
        {
            var existingResearchLine = await _repository.ResearchLine.GetByIdAsync(id);
            if (existingResearchLine == null)
            {
                throw new ArgumentException($"ResearchLine with id {id} does not exist.");
            }

            existingResearchLine = researchLineDto.ToEntity(existingResearchLine);
            await _repository.ResearchLine.UpdateAsync(existingResearchLine);

            return existingResearchLine.ToDto();
        }

        /// <inheritdoc />
        public async Task DeleteResearchLineAsync(Guid id)
        {
            var existingResearchLine = await _repository.ResearchLine.GetByIdAsync(id);
            if (existingResearchLine == null)
            {
                throw new ArgumentException($"ResearchLine with id {id} does not exist.");
            }

            await _repository.ResearchLine.DeactiveAsync(existingResearchLine);
        }
    }
}
