using saga.Infrastructure.Repositories;
using saga.Models.DTOs;

namespace saga.Infrastructure.Validations
{
    /// <summary>
    /// Provides validation methods for courses.
    /// </summary>
    public class CourseValidator
    {
        private readonly IRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CourseValidator"/> class.
        /// </summary>
        /// <param name="repository">The repository used for data access.</param>
        public CourseValidator(IRepository repository)
        {
            _repository = repository;
        }
    }
}
