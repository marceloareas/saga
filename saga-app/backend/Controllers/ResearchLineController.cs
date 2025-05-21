using saga.Models.DTOs;
using saga.Services;
using saga.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace saga.Controllers
{
    [ApiController]
    [Route("researchLines")]
    public class ResearchLineController : ControllerBase
    {
        private readonly IResearchLineService _researchLineService;

        public ResearchLineController(IResearchLineService researchLineService)
        {
            _researchLineService = researchLineService;
        }

        /// <summary>
        /// Creates a new researchLine.
        /// </summary>
        /// <param name="researchLineDto">The researchLine data.</param>
        /// <returns>The created researchLine.</returns>
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<ResearchLineInfoDto>> CreateResearchLine(ResearchLineDto researchLineDto)
        {
            try
            {
                var researchLine = await _researchLineService.CreateResearchLineAsync(researchLineDto);
                return CreatedAtAction(nameof(GetResearchLine), new { id = researchLine.Id }, researchLine);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets a researchLine by its ID.
        /// </summary>
        /// <param name="id">The researchLine ID.</param>
        /// <returns>The researchLine.</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<ResearchLineInfoDto>> GetResearchLine(Guid id)
        {
            try
            {
                var researchLine = await _researchLineService.GetResearchLineAsync(id);
                return Ok(researchLine);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets all researchLines.
        /// </summary>
        /// <returns>A list of researchLines.</returns>
        [HttpGet]
        [Authorize(Roles = "Administrator, Student, Professor")]
        [ProducesResponseType(typeof(IEnumerable<ResearchLineInfoDto>), 200)]
        public async Task<ActionResult<IEnumerable<ResearchLineInfoDto>>> GetAllResearchLinesAsync()
        {
            var researchLineDtos = await _researchLineService.GetAllResearchLinesAsync();
            return Ok(researchLineDtos);
        }

        /// <summary>
        /// Updates a researchLine by its ID.
        /// </summary>
        /// <param name="id">The researchLine ID.</param>
        /// <param name="researchLineDto">The researchLine data.</param>
        /// <returns>The updated researchLine.</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<ResearchLineInfoDto>> UpdateResearchLine(Guid id, ResearchLineDto researchLineDto)
        {
            try
            {
                var researchLineId = await _researchLineService.UpdateResearchLineAsync(id, researchLineDto);
                return CreatedAtAction(nameof(GetResearchLine), new { id = researchLineId });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a researchLine by its ID.
        /// </summary>
        /// <param name="id">The researchLine ID.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteResearchLine(Guid id)
        {
            try
            {
                await _researchLineService.DeleteResearchLineAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
