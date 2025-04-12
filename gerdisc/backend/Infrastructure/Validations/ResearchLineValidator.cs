using saga.Infrastructure.Repositories;
using saga.Models.DTOs;

namespace saga.Infrastructure.Validations
{
    /// <summary>
    /// Provides validation methods for researchLines.
    /// </summary>
    public class ResearchLineValidator
    {
        private readonly IRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResearchLineValidator"/> class.
        /// </summary>
        /// <param name="repository">The repository used for data access.</param>
        public ResearchLineValidator(IRepository repository)
        {
            _repository = repository;
        }
    }
}
