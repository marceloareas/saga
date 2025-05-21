using saga.Models.DTOs;

namespace saga.Services.Interfaces
{
    /// <summary>
    /// Defines methods for managing externalResearchers.
    /// </summary>
    public interface IExternalResearcherService
    {
        /// <summary>
        /// Creates a new externalResearcher.
        /// </summary>
        /// <param name="externalResearcherDto">The externalResearcher DTO containing the externalResearcher details.</param>
        /// <returns>The created externalResearcher DTO.</returns>
        Task<ExternalResearcherDto> CreateExternalResearcherAsync(ExternalResearcherDto externalResearcherDto);

        /// <summary>
        /// Retrieves a externalResearcher by ID.
        /// </summary>
        /// <param name="id">The ID of the externalResearcher to retrieve.</param>
        /// <returns>The externalResearcher DTO with the specified ID.</returns>
        Task<ExternalResearcherDto> GetExternalResearcherAsync(Guid id);

        /// <summary>
        /// Gets a list of all externalResearcher entities.
        /// </summary>
        /// <returns>A list of all externalResearcher entities.</returns>
        Task<IEnumerable<ExternalResearcherDto>> GetAllExternalResearchersAsync();

        /// <summary>
        /// Updates a externalResearcher.
        /// </summary>
        /// <param name="id">The ID of the externalResearcher to update.</param>
        /// <param name="externalResearcherDto">The externalResearcher DTO containing the updated externalResearcher details.</param>
        /// <returns>The updated externalResearcher DTO.</returns>
        Task<ExternalResearcherDto> UpdateExternalResearcherAsync(Guid id, ExternalResearcherDto externalResearcherDto);

        /// <summary>
        /// Deletes a externalResearcher.
        /// </summary>
        /// <param name="id">The ID of the externalResearcher to delete.</param>
        Task DeleteExternalResearcherAsync(Guid id);
    }
}
