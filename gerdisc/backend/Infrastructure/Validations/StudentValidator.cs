using saga.Infrastructure.Repositories;
using saga.Models.DTOs;

namespace saga.Infrastructure.Validations
{
    /// <summary>
    /// Provides validation methods for students.
    /// </summary>
    public class StudentValidator
    {
        private readonly IRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentValidator"/> class.
        /// </summary>
        /// <param name="repository">The repository used for data access.</param>
        public StudentValidator(IRepository repository)
        {
            _repository = repository;
        }
    }
}
