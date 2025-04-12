using saga.Models.DTOs;

namespace saga.Services.Interfaces
{
    /// <summary>
    /// Defines methods for managing researchLines.
    /// </summary>
    public interface IResearchLineService
    {
        /// <summary>
        /// Creates a new researchLine.
        /// </summary>
        /// <param name="researchLineDto">The researchLine DTO containing the researchLine details.</param>
        /// <returns>The created researchLine DTO.</returns>
        Task<ResearchLineInfoDto> CreateResearchLineAsync(ResearchLineDto researchLineDto);

        /// <summary>
        /// Retrieves a researchLine by ID.
        /// </summary>
        /// <param name="id">The ID of the researchLine to retrieve.</param>
        /// <returns>The researchLine DTO with the specified ID.</returns>
        Task<ResearchLineInfoDto> GetResearchLineAsync(Guid id);

        /// <summary>
        /// Gets a list of all researchLine entities.
        /// </summary>
        /// <returns>A list of all researchLine entities.</returns>
        Task<IEnumerable<ResearchLineInfoDto>> GetAllResearchLinesAsync();

        /// <summary>
        /// Updates a researchLine.
        /// </summary>
        /// <param name="id">The ID of the researchLine to update.</param>
        /// <param name="researchLineDto">The researchLine DTO containing the updated researchLine details.</param>
        /// <returns>The updated researchLine DTO.</returns>
        Task<ResearchLineInfoDto> UpdateResearchLineAsync(Guid id, ResearchLineDto researchLineDto);

        /// <summary>
        /// Deletes a researchLine.
        /// </summary>
        /// <param name="id">The ID of the researchLine to delete.</param>
        Task DeleteResearchLineAsync(Guid id);
    }
}
