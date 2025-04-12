using saga.Infrastructure.Providers;
using saga.Models.Entities;
using saga.Models.Enums;

namespace saga.Infrastructure.Extensions
{
    public static class ProjectRepositoryExtensions
    {
        public static IQueryable<ProjectEntity> FilterByUserRole(this IQueryable<ProjectEntity> query, IUserContext userContext)
        {
            switch (userContext.Role)
            {
                case RolesEnum.Professor:
                    return query.Where(
                        p => p.ProfessorProjects.Any(professor => professor.ProfessorId == userContext.UserId) ||
                            p.Orientations.Any(x => x.ProfessorId == userContext.UserId));
                case RolesEnum.Student:
                    return query.Where(p => p.Students.Any(student => student.Id == userContext.UserId));
                case RolesEnum.Administrator:
                    return query;
                case RolesEnum.ExternalResearcher:
                    return query.Where(d => d.Orientations.Any(x => x.CoorientatorId == userContext.UserId));
                default:
                    return query.Where(d => false);
            }
        }
    }
}
