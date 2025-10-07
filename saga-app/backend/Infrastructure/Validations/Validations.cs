using saga.Infrastructure.Providers;
using saga.Infrastructure.Repositories;
using saga.Infrastructure.Validations;

namespace saga.Infrastructure.Validations
{
    public class Validations
    {
        private readonly IRepository _repository;
        private readonly IUserContext _userContext;

        public Validations(
            IRepository repository,
            IUserContext userContext,
            UserValidator userValidator,
            OrientationValidator orientationValidator,
            StudentValidator studentValidator,
            ProfessorValidator professorValidator   
        )
        {
            _repository = repository;
            _userContext = userContext;
            UserValidator = userValidator;
            OrientationValidator = orientationValidator;
            StudentValidator = studentValidator;
            ProfessorValidator = professorValidator;
        }

        public OrientationValidator OrientationValidator { get; }
        public UserValidator        UserValidator        { get; }
        public StudentValidator     StudentValidator     { get; }
        public ProfessorValidator   ProfessorValidator   { get; }
    }
}
