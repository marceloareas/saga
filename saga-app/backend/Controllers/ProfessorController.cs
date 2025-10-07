using saga.Models.DTOs;
using saga.Services;
using saga.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using saga.Models.DTOs.Common;

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

        /// <param name="page">Página (>= 1)</param>
        /// <param name="pageSize">Tamanho da página (1–200)</param>
        /// <param name="q">Busca simples por nome ou e-mail</param>
        [HttpGet("paged")]
        [ProducesResponseType(typeof(PagedResult<ProfessorInfoDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<ProfessorInfoDto>>> GetProfessorsPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50,
            [FromQuery] string? q = null)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 50;
            if (pageSize > 200) pageSize = 200;
            var result = await _professorService.GetProfessorsPageAsync(page, pageSize, q);
            return Ok(result);
        }
    }
}
