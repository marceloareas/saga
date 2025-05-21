using saga.Infrastructure.Extensions;
using saga.Models.Entities;

namespace saga.Infrastructure.Repositories.ProfessorProject
{
    /// <inheritdoc />
    public class ProfessorProjectRepository : BaseRepository<ProfessorProjectEntity>, IProfessorProjectRepository
    {
        public ProfessorProjectRepository(ContexRepository dbContext) : base(dbContext)
        {
        }

        /// <inheritdoc />
        public async Task HandlesByProject(IEnumerable<Guid> newProfessors, ProjectEntity Project)
        {
            var professorProjects = await this.GetAllAsync(x => x.ProjectId == Project.Id);

            (var professorProjectIds, var professorProjectIdsToDelete) = professorProjects
                .Select(x => x.ProfessorId)
                .IEnumerableDifference(newProfessors);


            await this.AddRangeAsync(professorProjectIds.Select(
                x => new ProfessorProjectEntity
                {
                    ProfessorId = x,
                    ProjectId = Project.Id
                }
            ));
            await this.DeactiveRangeAsync(entity => professorProjectIdsToDelete.Contains(entity.ProfessorId));
        }

        /// <inheritdoc />
        public async Task HandleByProfessor(IEnumerable<Guid> newProjects, ProfessorEntity Professor)
        {
            var professorProjects = await this.GetAllAsync(x => x.ProfessorId == Professor.Id);

            (var professorProjectIds, var professorProjectIdsToDelete) = professorProjects
                .Select(x => x.ProjectId)
                .IEnumerableDifference(newProjects);


            await this.AddRangeAsync(professorProjectIds.Select(
                x => new ProfessorProjectEntity
                {
                    ProfessorId = Professor.UserId,
                    ProjectId = x
                }
            ));

            await this.DeactiveRangeAsync(entity => professorProjectIdsToDelete.Contains(entity.ProfessorId));
        }
    }
}
