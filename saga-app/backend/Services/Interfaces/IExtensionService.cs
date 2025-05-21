using saga.Models.DTOs;

namespace saga.Services.Interfaces
{
    /// <summary>
    /// Defines methods for managing extensions.
    /// </summary>
    public interface IExtensionService
    {
        /// <summary>
        /// Creates a new extension.
        /// </summary>
        /// <param name="extensionDto">The extension DTO containing the extension details.</param>
        /// <returns>The created extension DTO.</returns>
        Task<ExtensionInfoDto> CreateExtensionAsync(ExtensionDto extensionDto);

        /// <summary>
        /// Retrieves a extension by ID.
        /// </summary>
        /// <param name="id">The ID of the extension to retrieve.</param>
        /// <returns>The extension DTO with the specified ID.</returns>
        Task<ExtensionInfoDto> GetExtensionAsync(Guid id);

        /// <summary>
        /// Gets a list of all extension entities.
        /// </summary>
        /// <returns>A list of all extension entities.</returns>
        Task<IEnumerable<ExtensionInfoDto>> GetAllExtensionsAsync();

        /// <summary>
        /// Updates a extension.
        /// </summary>
        /// <param name="id">The ID of the extension to update.</param>
        /// <param name="extensionDto">The extension DTO containing the updated extension details.</param>
        /// <returns>The updated extension DTO.</returns>
        Task<ExtensionInfoDto> UpdateExtensionAsync(Guid id, ExtensionDto extensionDto);

        /// <summary>
        /// Deletes a extension.
        /// </summary>
        /// <param name="id">The ID of the extension to delete.</param>
        Task DeleteExtensionAsync(Guid id);
    }
}
