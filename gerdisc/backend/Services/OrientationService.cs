using backend.Infrastructure.Validations;
using saga.Infrastructure.Repositories;
using saga.Infrastructure.Validations;
using saga.Models.DTOs;
using saga.Models.Mapper;
using saga.Services.Interfaces;

namespace saga.Services
{
    public class OrientationService : IOrientationService
    {
        private readonly IRepository _repository;
        private readonly ILogger<OrientationService> _logger;
        private readonly Validations _validations;

        public OrientationService(
            IRepository repository,
            ILogger<OrientationService> logger,
            Validations validations
        )
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _validations = validations ?? throw new ArgumentNullException(nameof(validations));
        }

        /// <inheritdoc />
        public async Task<OrientationInfoDto> CreateOrientationAsync(OrientationDto orientationDto)
        {
            (var isValid, var message) = await _validations.OrientationValidator.CanAddOrientationToProject(orientationDto);
            if (!isValid)
            {
                throw new ArgumentException(message);
            }

            var orientation = await _repository.Orientation.AddAsync(orientationDto.ToEntity());

            _logger.LogInformation($"Orientation {orientation.Id} created successfully.");
            return orientation.ToInfoDto();
        }

        /// <inheritdoc />
        public async Task<OrientationInfoDto> GetOrientationAsync(Guid id)
        {
            var orientationEntity = await _repository.Orientation.GetByIdAsync(id, x => x.Professor, x => x.Coorientator, x => x.Student, x => x.Project);
            if (orientationEntity == null)
            {
                throw new ArgumentException("Orientation not found.");
            }

            return orientationEntity.ToInfoDto();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<OrientationInfoDto>> GetAllOrientationsAsync()
        {
            var orientations = await _repository.Orientation.GetAllAsync(x => x.Professor, x => x.Coorientator, x => x.Student, x => x.Project);
            var orientationDtos = new List<OrientationInfoDto>();
            foreach (var orientation in orientations)
            {
                orientationDtos.Add(orientation.ToInfoDto());
            }

            return orientationDtos;
        }

        /// <inheritdoc />
        public async Task<OrientationInfoDto> UpdateOrientationAsync(Guid id, OrientationDto orientationDto)
        {
            (var isValid, var message) = await _validations.OrientationValidator.CanAddOrientationToProject(orientationDto);
            if (!isValid)
            {
                throw new ArgumentException(message);
            }

            var existingOrientation = await _repository.Orientation.GetByIdAsync(id) ?? throw new ArgumentException($"Orientation with id {id} does not exist.");

            existingOrientation = orientationDto.ToEntity(existingOrientation);

            await _repository.Orientation.UpdateAsync(existingOrientation);

            return existingOrientation.ToInfoDto();
        }

        /// <inheritdoc />
        public async Task DeleteOrientationAsync(Guid id)
        {
            var existingOrientation = await _repository.Orientation.GetByIdAsync(id);
            if (existingOrientation == null)
            {
                throw new ArgumentException($"Orientation with id {id} does not exist.");
            }

            await _repository.Orientation.DeactiveAsync(existingOrientation);
        }
    }
}
