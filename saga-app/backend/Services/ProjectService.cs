using saga.Infrastructure.Extensions;
using saga.Infrastructure.Repositories;
using saga.Models.DTOs;
using saga.Models.Mapper;
using saga.Services.Interfaces;

namespace saga.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IRepository _repository;
        private readonly ILogger<ProjectService> _logger;

        public ProjectService(
            IRepository repository,
            ILogger<ProjectService> logger
        )
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public async Task<ProjectInfoDto> CreateProjectAsync(ProjectDto projectDto)
        {
            try
            {
                var project = projectDto.ToEntity();

                var professorIds = projectDto.ProfessorIds.Select(x => Guid.Parse(x));

                project = await _repository.Project.AddAsync(project);
                await _repository.ProfessorProject.HandlesByProject(projectDto.ProfessorIds.Select(Guid.Parse), project);

                _logger.LogInformation($"Project {project.Name} created successfully.");
                return project.ToInfoDto();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Project {projectDto.Name} as {ex}");
                return projectDto.ToEntity().ToInfoDto();
            };
        }

        /// <inheritdoc />
        public async Task<ProjectInfoDto> GetProjectAsync(Guid id)
        {
            var projectEntity = await _repository
                .Project
                .GetByIdAsync(id);
            if (projectEntity == null)
            {
                throw new ArgumentException("Project not found.");
            }

            return projectEntity.ToInfoDto();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ProjectInfoDto>> GetAllProjectsAsync()
        {
            var projects = await _repository
                .Project
                .GetAllAsync();
            var projectDtos = new List<ProjectInfoDto>();
            foreach (var project in projects)
            {
                projectDtos.Add(project.ToInfoDto());
            }

            return projectDtos;
        }

        /// <inheritdoc />
        public async Task<ProjectInfoDto> UpdateProjectAsync(Guid id, ProjectDto projectDto)
        {
            var existingProject = await _repository.Project.GetByIdAsync(id);
            if (existingProject == null)
            {
                throw new ArgumentException($"Project with id {id} does not exist.");
            }

            existingProject = projectDto.ToEntity(existingProject);
            await _repository.Project.UpdateAsync(existingProject);
            await _repository.ProfessorProject.HandlesByProject(projectDto.ProfessorIds.Select(Guid.Parse), existingProject);

            return existingProject.ToInfoDto();
        }

        /// <inheritdoc />
        public async Task DeleteProjectAsync(Guid id)
        {
            var existingProject = await _repository.Project.GetByIdAsync(id);
            if (existingProject == null)
            {
                throw new ArgumentException($"Project with id {id} does not exist.");
            }

            await _repository.Project.DeactiveAsync(existingProject);
        }
    }
}
