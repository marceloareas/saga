using saga.Models.DTOs;
using saga.Models.Entities;

namespace saga.Services.Interfaces
{
    /// <summary>
    /// provides the contract for user-related operations in the application.
    /// It defines several methods for creating, retrieving, updating, and deleting user information.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Creates a new user entity.
        /// </summary>
        /// <param name="userDto">The user entity to create.</param>
        /// <returns>The created user entity.</returns>
        Task<UserEntity> CreateUserAsync(UserDto userDto);

        /// <summary>
        /// Updates an existing user entity.
        /// </summary>
        /// <param name="id">The unique identifier of the user entity to update.</param
        /// <param name="userDto">The user entity with updated information.</param>
        /// <returns>The updated user entity.</returns>
        Task<UserEntity> UpdateUserAsync(Guid id, UserDto userDto);

        /// <summary>
        /// Deletes a user entity by its unique identifier.
        /// /// </summary>
        /// <param name="id">The unique identifier of the user entity to delete.</param>
        /// <returns>A task representing the asynchronous delete operation.</returns>
        Task<UserEntity> DeleteUserAsync(Guid id);

        /// <summary>
        /// Authenticate user with provided email and password
        /// </summary>
        /// <param name="loginDto">The LoginDto object containing email and password information</param>
        /// <returns>The generated JWT token</returns>
        Task<LoginResultDto> AuthenticateAsync(LoginDto loginDto);

        /// <summary>
        /// Requests a password reset for the user with the specified email.
        /// </summary>
        /// <param name="request">The request reset password data transfer object.</param>
        /// <returns>A task representing the asynchronous password reset request.</returns>
        /// <exception cref="ArgumentException">Thrown when the user with the specified email is not found.</exception>
        Task ResetPasswordRequestAsync(RequestResetPasswordDto request);

        /// <summary>
        /// Resets the password for the user with the specified email.
        /// </summary>
        /// <param name="loginDto">The reset password data transfer object.</param>
        /// <returns>A task representing the asynchronous password reset operation.</returns>
        /// <exception cref="ArgumentException">Thrown when the user with the specified email is not found.</exception>
        Task<LoginResultDto> ResetPasswordAsync(ResetPasswordDto loginDto);

        /// <summary>
        /// Authenticate user with provided email and password
        /// </summary>
        /// <param name="loginDto">The LoginDto object containing email and password information</param>
        /// <returns>The generated JWT token</returns>
        Task<LoginResultDto> AuthenticateAsync(LoginDto loginDto);

        
        
    }
}
