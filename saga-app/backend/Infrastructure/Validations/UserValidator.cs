using saga.Infrastructure.Providers;
using saga.Infrastructure.Repositories;
using saga.Models.DTOs;

namespace saga.Infrastructure.Validations
{
    /// <summary>
    /// Provides validation methods for users.
    /// </summary>
    public class UserValidator
    {
        private readonly IRepository _repository;
        private readonly IUserContext _userContext;
        private readonly ILogger<UserValidator> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserValidator"/> class.
        /// </summary>
        /// <param name="repository">The repository used for data access.</param>
        public UserValidator(IRepository repository, ILogger<UserValidator> logger, IUserContext userContext)
        {
            _repository = repository;
            _userContext = userContext;
            _logger = logger;
        }

        /// <summary>
        /// Checks if a user can be added based on the provided user DTO.
        /// </summary>
        /// <param name="userDto">The user DTO to be checked.</param>
        /// <returns>A tuple containing a boolean indicating if the user can be added and an error message if applicable.</returns>
        public async Task<(bool exists, string errorMessage)> CanAddUser(UserDto userDto)
        {
            _logger.LogInformation("starting verifi userDto");
            if (userDto is null || userDto.Email is null)
            {
                _logger.LogInformation("Invalid user DTO.");
                return (false, $"Invalid user DTO.");
            }

            var user = await _repository.User.GetUserByEmail(userDto.Email);

            if (user is not null)
            {
                _logger.LogInformation($"User with email '{userDto.Email}' already exists.");
                return (false, $"User with email '{userDto.Email}' already exists.");
            }

            _logger.LogInformation($"User dto verified.");
            return (true, "Success");
        }

        /// <summary>
        /// Checks if a user can reset password based on the provided ResetPassword DTO.
        /// </summary>
        /// <param name="resetPasswordDto">The ResetPassword DTO to be checked.</param>
        /// <returns>A tuple containing a boolean indicating if the user can reset password and an error message if applicable.</returns>
        public async Task<(bool exists, string errorMessage)> CanResetPassword(ResetPasswordDto resetPasswordDto)
        {
            if (_userContext.UserId is null)
            {
                _logger.LogInformation("Token.");
                return (false, $"Token.");
            }

            _logger.LogInformation($"reset password dto verified.");
            return (true, "Success");
        }
    }
}
