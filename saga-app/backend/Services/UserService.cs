using backend.Infrastructure.Validations;
using Infrastructure.EmailTemplates;
using saga.Infrastructure.Providers;
using saga.Infrastructure.Providers.Interfaces;
using saga.Infrastructure.Repositories;
using saga.Infrastructure.Validations;
using saga.Models.DTOs;
using saga.Models.Entities;
using saga.Models.Mapper;
using saga.Properties;
using saga.Services.Interfaces;

namespace saga.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository _repository;
        private readonly IEmailSender _emailSender;
        private readonly ITokenProvider _tokenProvider;
        private readonly IUserContext _userContext;
        private readonly ILogger<UserService> _logger;
        private readonly Validations _validations;
        public UserService(
            IRepository repository,
            ITokenProvider tokenProvider,
            ILogger<UserService> logger,
            IEmailSender emailSender,
            Validations validations,
            IUserContext userContext
        )
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _validations = validations ?? throw new ArgumentNullException(nameof(validations));
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
        }

        /// <inheritdoc />
        public async Task<UserEntity> CreateUserAsync(UserDto userDto)
        {
            (var isValid, var message) = await _validations.UserValidator.CanAddUser(userDto);

            _logger.LogInformation($"Creating user with email: {userDto.Email}");

            var existingUser = await _repository.User.GetUserByEmail(userDto.Email);
            if (existingUser != null)
            {
                await _repository.User.ActivateAsync(existingUser);
                _logger.LogInformation($"User {userDto.Email} already exists, reactivating.");
                return existingUser;
            }

            _logger.LogInformation($"Creating user{userDto.Email}");
            if (!isValid)
            {
                throw new ArgumentException(message);
            }

            var user = await _repository.User.AddAsync(userDto.ToUserEntity());
            var token = _tokenProvider.GenerateResetPasswordJwt(user, TimeSpan.FromDays(7));
            string emailSubject = "Sua conta foi criada";
            string emailBody = EmailTemplates.WelcomeEmailTemplate(userDto.ResetPasswordPath, token);
            await _emailSender.SendEmail(userDto.Email, emailSubject, emailBody).ConfigureAwait(false);

            return user;
        }

        /// <inheritdoc />
        public async Task<UserEntity> UpdateUserAsync(Guid id, UserDto userDto)
        {
            var existingUser = await _repository
                .User
                .GetByIdAsync(id) ?? throw new ArgumentException($"User with id {id} not found.");
            existingUser = userDto.ToUserEntity(existingUser);
            await _repository.User.UpdateAsync(existingUser);

            _logger.LogInformation($"User {existingUser.Email} updated successfully. New data: {string.Join(", ", userDto.GetType().GetProperties().Select(p => $"{p.Name}: {p.GetValue(userDto)}"))}");

            return existingUser;
        }

        /// <inheritdoc />
        public async Task<UserEntity> DeleteUserAsync(Guid id)
        {
            var existingUser = await _repository
                .User
                .GetByIdAsync(id) ?? throw new ArgumentException($"User with id {id} not found.");
            await _repository.User.DeactiveAsync(existingUser);
            _logger.LogInformation($"User {existingUser.Email} deactivated successfully.");
            return existingUser;
        }

        /// <inheritdoc />
        public async Task ResetPasswordRequestAsync(RequestResetPasswordDto request)
        {
            var user = await _repository.User.GetUserByEmail(request.Email) ?? throw new ArgumentException($"User with email {request.Email} not found.");

            var token = _tokenProvider.GenerateResetPasswordJwt(user, TimeSpan.FromMinutes(30));
            string emailSubject = "Alteração de senha";
            string emailBody = EmailTemplates.ResetPasswordEmailTemplate(request.ResetPasswordPath, token);
            await _emailSender.SendEmail(request.Email, emailSubject, emailBody).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<LoginResultDto> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            (var isValid, var message) = await _validations.UserValidator.CanResetPassword(resetPasswordDto);

            if (!isValid)
            {
                throw new ArgumentException(message);
            }
            var user = await _repository.User.GetByIdAsync(_userContext.UserId.Value) ?? throw new ArgumentException($"User with email {_userContext.UserId} not found.");

            _logger.LogInformation($"Changing password of user: {user.Email}");
            user.PasswordHash = HashPassword(resetPasswordDto.Password);

            await _repository.User.UpdateAsync(user);

            var jwtToken = _tokenProvider.GenerateJwtToken(user);

            return user.ToDto(jwtToken);
        }


        /// <inheritdoc />
        public async Task<LoginResultDto> AuthenticateAsync(LoginDto loginDto)
        {
            var user = await _repository.User.GetUserByEmail(loginDto.Email.ToLower());
            if (user == null)
            {
                throw new ArgumentException($"User with email {loginDto.Email} not found.");
            }

            if (!VerifyPassword(loginDto.Password ?? "", user.PasswordHash ?? ""))
            {
                throw new ArgumentException("Invalid password.");
            }

            return user.ToDto(_tokenProvider.GenerateJwtToken(user));
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
