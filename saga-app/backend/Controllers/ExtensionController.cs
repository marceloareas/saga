using saga.Infrastructure.Repositories;
using saga.Models.DTOs;
using saga.Services;
using saga.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace saga.Controllers
{
    [ApiController]
    [Route("extensions")]
    public class ExtensionController : ControllerBase
    {
        private readonly IExtensionService _extensionService;

        public ExtensionController(IExtensionService extensionService)
        {
            _extensionService = extensionService;
        }

        /// <summary>
        /// Creates a new extension.
        /// </summary>
        /// <param name="extensionDto">The extension data.</param>
        /// <returns>The created extension.</returns>
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<ExtensionInfoDto>> CreateExtension(ExtensionDto extensionDto)
        {
            try
            {
                var extension = await _extensionService.CreateExtensionAsync(extensionDto);
                return CreatedAtAction(nameof(GetExtension), new { id = extension.Id }, extension);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets a extension by its ID.
        /// </summary>
        /// <param name="id">The extension ID.</param>
        /// <returns>The extension.</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator, Student, Professor, ExternalResearcher")]
        public async Task<ActionResult<ExtensionInfoDto>> GetExtension(Guid id)
        {
            try
            {
                var extension = await _extensionService.GetExtensionAsync(id);
                return Ok(extension);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ExtensionInfoDto>), StatusCodes.Status200OK)]
        [Authorize(Roles = "Administrator, Student")]
        public async Task<ActionResult<IEnumerable<ExtensionInfoDto>>> GetAllExtensionsAsync()
        {
            var extensionDtos = await _extensionService.GetAllExtensionsAsync();

            return Ok(extensionDtos);
        }

        /// <summary>
        /// Updates a extension by its ID.
        /// </summary>
        /// <param name="id">The extension ID.</param>
        /// <param name="extensionDto">The extension data.</param>
        /// <returns>The updated extension.</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<ExtensionInfoDto>> UpdateExtension(Guid id, ExtensionDto extensionDto)
        {
            try
            {
                var extension = await _extensionService.UpdateExtensionAsync(id, extensionDto);
                return Ok(extension);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a extension by its ID.
        /// </summary>
        /// <param name="id">The extension ID.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteExtension(Guid id)
        {
            try
            {
                await _extensionService.DeleteExtensionAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
