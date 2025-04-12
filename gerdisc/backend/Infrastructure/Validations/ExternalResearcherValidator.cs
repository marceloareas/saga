using saga.Infrastructure.Repositories;
using saga.Models.DTOs;

namespace saga.Infrastructure.Validations
{
    /// <summary>
    /// Provides validation methods for externalResearchers.
    /// </summary>
    public class ExternalResearcherValidator
    {
        private readonly IRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalResearcherValidator"/> class.
        /// </summary>
        /// <param name="repository">The repository used for data access.</param>
        public ExternalResearcherValidator(IRepository repository)
        {
            _repository = repository;
        }
    }
}
