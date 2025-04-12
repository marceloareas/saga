using saga.Models.Entities;

namespace saga.Infrastructure.Repositories.ProfessorProject
{
    /// <inheritdoc />
    public interface IProfessorProjectRepository : IBaseRepository<ProfessorProjectEntity>
    {
        /// <summary>
        /// Handles the association between professors and a project by adding or removing the appropriate records.
        /// </summary>
        /// <param name="newProfessors">The collection of professor IDs to associate with the project.</param>
        /// <param name="project">The project entity to handle the associations for.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task HandlesByProject(IEnumerable<Guid> newProfessors, ProjectEntity Project);

        /// <summary>
        /// Handles the association between projects and a Professor by adding or removing the appropriate records.
        /// </summary>
        /// <param name="newProjects">The collection of project IDs to associate with the Professor.</param>
        /// <param name="Professor">The Professor entity to handle the associations for.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task HandleByProfessor(IEnumerable<Guid> newProjects, ProfessorEntity Professor);
    }
}
