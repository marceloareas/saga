using saga.Infrastructure.Repositories;
using saga.Models.DTOs;
using saga.Models.Entities;
using saga.Models.Enums;
using saga.Models.Mapper;
using saga.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace saga.Services
{
    public class ExtensionService : IExtensionService
    {
        private readonly IRepository _repository;
        private readonly ILogger<ExtensionService> _logger;

        public ExtensionService(
            IRepository repository,
            ILogger<ExtensionService> logger
        )
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public async Task<ExtensionInfoDto> CreateExtensionAsync(ExtensionDto extensionDto)
        {
            var extension = extensionDto.ToEntity();

            var student = await _repository.Student.GetByIdAsync(extension.StudentId);

            if (student is null)
            {
                throw new ArgumentException($"Student with id: {extension.StudentId} does not exist.");
            }

            extension = await _repository.Extension.AddAsync(extension);

            UpdateUserDates(student, extension);

            await _repository.Student.UpdateAsync(student);

            _logger.LogInformation($"Extension {extension.StudentId} created successfully.");
            return extension.ToDto();
        }

        /// <inheritdoc />
        public async Task<ExtensionInfoDto> GetExtensionAsync(Guid id)
        {
            var extensionEntity = await _repository.Extension.GetByIdAsync(id, x => x.Student);
            if (extensionEntity == null)
            {
                throw new ArgumentException("Extension not found.");
            }

            return extensionEntity.ToDto();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ExtensionInfoDto>> GetAllExtensionsAsync()
        {
            var extensions = await _repository.Extension.GetAllAsync(x => x.Student);
            var extensionDtos = new List<ExtensionInfoDto>();
            foreach (var extension in extensions)
            {
                extensionDtos.Add(extension.ToDto());
            }

            return extensionDtos;
        }

        /// <inheritdoc />
        public async Task<ExtensionInfoDto> UpdateExtensionAsync(Guid id, ExtensionDto extensionDto)
        {
            var existingExtension = await _repository.Extension.GetByIdAsync(id);
            var student = await _repository.Student.GetByIdAsync(extensionDto.StudentId);

            if (existingExtension == null || student is null)
            {
                throw new ArgumentException($"Extension with id {id} does not exist.");
            }

            var oldDays = existingExtension.NumberOfDays;

            existingExtension = extensionDto.ToEntity(existingExtension);

            await _repository.Extension.UpdateAsync(existingExtension);

            UpdateUserDates(student, existingExtension, oldDays);

            await _repository.Student.UpdateAsync(student);

            return existingExtension.ToDto();
        }

        /// <inheritdoc />
        public async Task DeleteExtensionAsync(Guid id)
        {
            var existingExtension = await _repository.Extension.GetByIdAsync(id);
            if (existingExtension == null)
            {
                throw new ArgumentException($"Extension with id {id} does not exist.");
            }

            await _repository.Extension.DeactiveAsync(existingExtension);
        }

        private async void UpdateUserDates(StudentEntity student, ExtensionEntity extension, int oldDays = 0)
        {
            switch (extension.Type)
            {
                case ExtensionTypeEnum.Qualification:
                    student.ProjectQualificationDate += TimeSpan.FromDays(extension.NumberOfDays - oldDays);
                    break;
                case ExtensionTypeEnum.Defence:
                    student.ProjectDefenceDate += TimeSpan.FromDays(extension.NumberOfDays - oldDays);
                    break;
                default:
                    break;
            }
        }
    }
}
