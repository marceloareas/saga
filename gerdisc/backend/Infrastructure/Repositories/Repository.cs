using saga.Infrastructure.Providers;
using saga.Infrastructure.Repositories.Course;
using saga.Infrastructure.Repositories.Extension;
using saga.Infrastructure.Repositories.ExternalResearcher;
using saga.Infrastructure.Repositories.Orientation;
using saga.Infrastructure.Repositories.Professor;
using saga.Infrastructure.Repositories.ProfessorProject;
using saga.Infrastructure.Repositories.Project;
using saga.Infrastructure.Repositories.ResearchLine;
using saga.Infrastructure.Repositories.Student;
using saga.Infrastructure.Repositories.StudentCourse;
using saga.Infrastructure.Repositories.User;
using saga.Infrastructure.Repositories.PondocQualis;

namespace saga.Infrastructure.Repositories
{
    /// <summary>
    /// Access to all entities in the database.
    /// </summary>
    public class Repository : IRepository
    {
        public ContexRepository _dbContext;
        private readonly IUserContext _userContext;

        /// <summary>
        /// Default constructor to create the data base context to access the database.
        /// </summary>
        /// <param name="server">Database url.</param>
        /// <param name="login">Database login.</param>
        /// <param name="password">Database password.</param>
        /// <param name="database">Database name.</param>
        public Repository(ContexRepository? dbContext, IUserContext userContext)
        {
            _userContext = userContext;
            if (dbContext is not null)
            {
                _dbContext = dbContext;
                _dbContext.Database.EnsureCreated();
            }
            else
            {
                throw new NullReferenceException("dbContext cannot be null.");
            }
        }


        /// <inheritdoc />
        public IUserRepository User => new UserRepository(_dbContext);

        /// <inheritdoc />
        public IStudentRepository Student => new StudentRepository(_dbContext, _userContext);

        /// <inheritdoc />
        public IProfessorRepository Professor => new ProfessorRepository(_dbContext);

        /// <inheritdoc />
        public IProjectRepository Project => new ProjectRepository(_dbContext, _userContext);

        /// <inheritdoc />
        public IExtensionRepository Extension => new ExtensionRepository(_dbContext, _userContext);

        /// <inheritdoc />
        public IExternalResearcherRepository ExternalResearcher => new ExternalResearcherRepository(_dbContext);

        /// <inheritdoc />
        public ICourseRepository Course => new CourseRepository(_dbContext);

        /// <inheritdoc />
        public IProfessorProjectRepository ProfessorProject => new ProfessorProjectRepository(_dbContext);

        /// <inheritdoc />
        public IStudentCourseRepository StudentCourse => new StudentCourseRepository(_dbContext);

        /// <inheritdoc />
        public IOrientationRepository Orientation => new OrientationRepository(_dbContext, _userContext);

        /// <inheritdoc />
        public IResearchLineRepository ResearchLine => new ResearchLineRepository(_dbContext);

        /// <inheritdoc />
        public IPondocQualisRepository PondocQualis => new PondocQualisRepository(_dbContext);

        /// <inheritdoc />
        public async Task<int> CommitAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
