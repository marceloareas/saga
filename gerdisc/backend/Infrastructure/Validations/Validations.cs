using saga.Infrastructure.Providers;
using saga.Infrastructure.Repositories;
using saga.Infrastructure.Validations;

namespace backend.Infrastructure.Validations
{
    /// <summary>
    /// Provides a collection of validators used for data validation.
    /// </summary>
    public class Validations
    {
        private readonly IRepository _repository;
        private readonly IUserContext _userContext;
        private readonly ILogger<UserValidator> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="Validations"/> class.
        /// </summary>
        /// <param name="repository">The repository used for data access.</param>
        /// <param name="logger">The logger used for logging validation errors.</param>
        /// <param name="userContext">The user context used for validation that requires user information.</param>
        public Validations(IRepository repository, ILogger<UserValidator> logger, IUserContext userContext)
        {
            _repository = repository;
            _userContext = userContext;
            _logger = logger;
        }

        /// <summary>
        /// Gets the validator for orientation-related validation.
        /// </summary>
        public OrientationValidator OrientationValidator => new OrientationValidator(_repository);

        /// <summary>
        /// Gets the validator for user-related validation.
        /// </summary>
        public UserValidator UserValidator => new UserValidator(_repository, _logger, _userContext);
    }
}
