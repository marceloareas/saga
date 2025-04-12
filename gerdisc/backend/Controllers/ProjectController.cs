using saga.Models.DTOs;
using saga.Services;
using saga.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace saga.Controllers
{
    [ApiController]
    [Route("projects")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="projectDto">The project data.</param>
        /// <returns>The created project.</returns>
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<ProjectInfoDto>> CreateProject(ProjectDto projectDto)
        {
            try
            {
                var project = await _projectService.CreateProjectAsync(projectDto);
                return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets a project by its ID.
        /// </summary>
        /// <param name="id">The project ID.</param>
        /// <returns>The project.</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator, Student, Professor, ExternalResearcher")]
        public async Task<ActionResult<ProjectInfoDto>> GetProject(Guid id)
        {
            try
            {
                var project = await _projectService.GetProjectAsync(id);
                return Ok(project);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets all projects.
        /// </summary>
        /// <returns>A list of projects.</returns>
        [HttpGet]
        [Authorize(Roles = "Administrator, Student, Professor, ExternalResearcher")]
        [ProducesResponseType(typeof(IEnumerable<ProjectInfoDto>), 200)]
        public async Task<ActionResult<IEnumerable<ProjectInfoDto>>> GetAllProjectsAsync()
        {
            var projectDtos = await _projectService.GetAllProjectsAsync();
            return Ok(projectDtos);
        }

        /// <summary>
        /// Updates a project by its ID.
        /// </summary>
        /// <param name="id">The project ID.</param>
        /// <param name="projectDto">The project data.</param>
        /// <returns>The updated project.</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<ProjectInfoDto>> UpdateProject(Guid id, ProjectDto projectDto)
        {
            try
            {
                var project = await _projectService.UpdateProjectAsync(id, projectDto);
                return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a project by its ID.
        /// </summary>
        /// <param name="id">The project ID.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            try
            {
                await _projectService.DeleteProjectAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
