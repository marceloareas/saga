using saga.Models.DTOs;
using saga.Services;
using saga.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace saga.Controllers
{
    [ApiController]
    [Route("externalResearchers")]
    public class ExternalResearcherController : ControllerBase
    {
        private readonly IExternalResearcherService _externalResearcherService;

        public ExternalResearcherController(IExternalResearcherService externalResearcherService)
        {
            _externalResearcherService = externalResearcherService;
        }

        /// <summary>
        /// Creates a new externalResearcher.
        /// </summary>
        /// <param name="externalResearcherDto">The externalResearcher data.</param>
        /// <returns>The created externalResearcher.</returns>
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<ExternalResearcherDto>> CreateExternalResearcher(ExternalResearcherDto externalResearcherDto)
        {
            try
            {
                var externalResearcher = await _externalResearcherService.CreateExternalResearcherAsync(externalResearcherDto);
                return CreatedAtAction(nameof(GetExternalResearcher), new { id = externalResearcher.Id }, externalResearcher);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets a externalResearcher by its ID.
        /// </summary>
        /// <param name="id">The externalResearcher ID.</param>
        /// <returns>The externalResearcher.</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<ExternalResearcherDto>> GetExternalResearcher(Guid id)
        {
            try
            {

                var externalResearcher = await _externalResearcherService.GetExternalResearcherAsync(id);
                return Ok(externalResearcher);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ExternalResearcherDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ExternalResearcherDto>>> GetAllExternalResearchersAsync()
        {
            var externalResearcherDtos = await _externalResearcherService.GetAllExternalResearchersAsync();

            return Ok(externalResearcherDtos);
        }

        /// <summary>
        /// Updates a externalResearcher by its ID.
        /// </summary>
        /// <param name="id">The externalResearcher ID.</param>
        /// <param name="externalResearcherDto">The externalResearcher data.</param>
        /// <returns>The updated externalResearcher.</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<ExternalResearcherDto>> UpdateExternalResearcher(Guid id, ExternalResearcherDto externalResearcherDto)
        {
            try
            {
                var externalResearcher = await _externalResearcherService.UpdateExternalResearcherAsync(id, externalResearcherDto);
                return Ok(externalResearcher);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a externalResearcher by its ID.
        /// </summary>
        /// <param name="id">The externalResearcher ID.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteExternalResearcher(Guid id)
        {
            try
            {
                await _externalResearcherService.DeleteExternalResearcherAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
