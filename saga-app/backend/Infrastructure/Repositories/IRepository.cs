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
    /// Represents a repository interface that provides access to various entity repositories.
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Gets the repository for user entities.
        /// </summary>
        IUserRepository User { get; }

        /// <summary>
        /// Gets the repository for student entities.
        /// </summary>
        IStudentRepository Student { get; }

        /// <summary>
        /// Gets the repository for professor entities.
        /// </summary>
        IProfessorRepository Professor { get; }

        /// <summary>
        /// Gets the repository for project entities.
        /// </summary>
        IProjectRepository Project { get; }

        /// <summary>
        /// Gets the repository for extension entities.
        /// </summary>
        IExtensionRepository Extension { get; }

        /// <summary>
        /// Gets the repository for external researcher entities.
        /// </summary>
        IExternalResearcherRepository ExternalResearcher { get; }

        /// <summary>
        /// Gets the repository for course entities.
        /// </summary>
        ICourseRepository Course { get; }

        /// <summary>
        /// Gets the repository for professor-project entities.
        /// </summary>
        IProfessorProjectRepository ProfessorProject { get; }

        /// <summary>
        /// Gets the repository for student-course entities.
        /// </summary>
        IStudentCourseRepository StudentCourse { get; }

        /// <summary>
        /// Gets the repository for orientation entities.
        /// </summary>
        IOrientationRepository Orientation { get; }

        /// <summary>
        /// Gets the repository for research line entities.
        /// </summary>
        IResearchLineRepository ResearchLine { get; }

        /// <summary>
        /// Gets the repository for Pondoc Qualis entities.
        /// </summary>
        IPondocQualisRepository PondocQualis { get; }

        /// <summary>
        /// Commits the pending changes asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous commit operation.</returns>
        Task<int> CommitAsync();
    }
}
