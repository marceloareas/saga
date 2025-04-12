using saga.Models.DTOs;
using saga.Services;
using saga.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace saga.Controllers
{
    [ApiController]
    [Route("professors")]
    public class ProfessorController : ControllerBase
    {
        private readonly IProfessorService _professorService;
        private readonly ILogger<ProfessorController> _logger;

        public ProfessorController(IProfessorService professorService, ILogger<ProfessorController> logger)
        {
            _professorService = professorService;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new professor.
        /// </summary>
        /// <param name="professorDto">The professor data.</param>
        /// <returns>The created professor.</returns>
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<ProfessorInfoDto>> CreateProfessor(ProfessorDto professorDto)
        {
            try
            {
                var professor = await _professorService.CreateProfessorAsync(professorDto);
                return CreatedAtAction(nameof(GetProfessor), new { id = professor.Id }, professor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets a professor by its ID.
        /// </summary>
        /// <param name="id">The professor ID.</param>
        /// <returns>The professor.</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<ProfessorInfoDto>> GetProfessor(Guid id)
        {
            try
            {
                var professor = await _professorService.GetProfessorAsync(id);
                return Ok(professor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred: {ErrorMessage}. Stack trace: {StackTrace}", ex.Message, ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProfessorInfoDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProfessorInfoDto>>> GetAllProfessorsAsync()
        {
            var professorDtos = await _professorService.GetAllProfessorsAsync();

            return Ok(professorDtos);
        }

        /// <summary>
        /// Updates a professor by its ID.
        /// </summary>
        /// <param name="id">The professor ID.</param>
        /// <param name="professorDto">The professor data.</param>
        /// <returns>The updated professor.</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<ProfessorInfoDto>> UpdateProfessor(Guid id, ProfessorDto professorDto)
        {
            try
            {
                var professor = await _professorService.UpdateProfessorAsync(id, professorDto);
                return CreatedAtAction(nameof(GetProfessor), new { id = professor.Id }, professor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred: {ErrorMessage}. Stack trace: {StackTrace}", ex.Message, ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a professor by its ID.
        /// </summary>
        /// <param name="id">The professor ID.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteProfessor(Guid id)
        {
            try
            {
                await _professorService.DeleteProfessorAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
