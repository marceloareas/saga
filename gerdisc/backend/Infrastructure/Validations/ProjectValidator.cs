using saga.Infrastructure.Repositories;
using saga.Models.DTOs;

namespace saga.Infrastructure.Validations
{
    /// <summary>
    /// Provides validation methods for projects.
    /// </summary>
    public class ProjectValidator
    {
        private readonly IRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectValidator"/> class.
        /// </summary>
        /// <param name="repository">The repository used for data access.</param>
        public ProjectValidator(IRepository repository)
        {
            _repository = repository;
        }
    }
}
