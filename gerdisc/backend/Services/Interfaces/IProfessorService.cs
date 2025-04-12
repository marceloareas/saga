using saga.Models.DTOs;

namespace saga.Services.Interfaces
{
    /// <summary>
    /// Defines methods for managing professors.
    /// </summary>
    public interface IProfessorService
    {
        /// <summary>
        /// Creates a new professor.
        /// </summary>
        /// <param name="professorDto">The professor DTO containing the professor details.</param>
        /// <returns>The created professor DTO.</returns>
        Task<ProfessorInfoDto> CreateProfessorAsync(ProfessorDto professorDto);

        /// <summary>
        /// Retrieves a professor by ID.
        /// </summary>
        /// <param name="id">The ID of the professor to retrieve.</param>
        /// <returns>The professor DTO with the specified ID.</returns>
        Task<ProfessorInfoDto> GetProfessorAsync(Guid id);

        /// <summary>
        /// Gets a list of all professor entities.
        /// </summary>
        /// <returns>A list of all professor entities.</returns>
        Task<IEnumerable<ProfessorInfoDto>> GetAllProfessorsAsync();

        /// <summary>
        /// Updates a professor.
        /// </summary>
        /// <param name="id">The ID of the professor to update.</param>
        /// <param name="professorDto">The professor DTO containing the updated professor details.</param>
        /// <returns>The updated professor DTO.</returns>
        Task<ProfessorInfoDto> UpdateProfessorAsync(Guid id, ProfessorDto professorDto);

        /// <summary>
        /// Deletes a professor.
        /// </summary>
        /// <param name="id">The ID of the professor to delete.</param>
        Task DeleteProfessorAsync(Guid id);
    }
}
