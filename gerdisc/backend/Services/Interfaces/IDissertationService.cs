using saga.Models.DTOs;

namespace saga.Services.Interfaces
{
    /// <summary>
    /// Defines methods for managing orientations.
    /// </summary>
    public interface IOrientationService
    {
        /// <summary>
        /// Creates a new orientation.
        /// </summary>
        /// <param name="orientationDto">The orientation DTO containing the orientation details.</param>
        /// <returns>The created orientation DTO.</returns>
        Task<OrientationInfoDto> CreateOrientationAsync(OrientationDto orientationDto);

        /// <summary>
        /// Retrieves a orientation by ID.
        /// </summary>
        /// <param name="id">The ID of the orientation to retrieve.</param>
        /// <returns>The orientation DTO with the specified ID.</returns>
        Task<OrientationInfoDto> GetOrientationAsync(Guid id);

        /// <summary>
        /// Gets a list of all orientation entities.
        /// </summary>
        /// <returns>A list of all orientation entities.</returns>
        Task<IEnumerable<OrientationInfoDto>> GetAllOrientationsAsync();

        /// <summary>
        /// Updates a orientation.
        /// </summary>
        /// <param name="id">The ID of the orientation to update.</param>
        /// <param name="orientationDto">The orientation DTO containing the updated orientation details.</param>
        /// <returns>The updated orientation DTO.</returns>
        Task<OrientationInfoDto> UpdateOrientationAsync(Guid id, OrientationDto orientationDto);

        /// <summary>
        /// Deletes a orientation.
        /// </summary>
        /// <param name="id">The ID of the orientation to delete.</param>
        Task DeleteOrientationAsync(Guid id);
    }
}
