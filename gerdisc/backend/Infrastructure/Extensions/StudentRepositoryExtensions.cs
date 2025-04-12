using saga.Infrastructure.Providers;
using saga.Models.Entities;
using saga.Models.Enums;

namespace saga.Infrastructure.Extensions
{
    public static class StudentRepositoryExtensions
    {
        public static IQueryable<StudentEntity> FilterByUserRole(this IQueryable<StudentEntity> query, IUserContext userContext)
        {
            switch (userContext.Role)
            {
                case RolesEnum.Professor:
                    return query.Where(p => p.Project == null ? false : p.Project.Orientations.Any(x => x.ProfessorId == userContext.UserId));
                case RolesEnum.Student:
                    return query.Where(p => p.Id == userContext.UserId);
                case RolesEnum.Administrator:
                    return query;
                case RolesEnum.ExternalResearcher:
                    return query.Where(d => d.Project == null ? false : d.Project.Orientations.Any(x => x.CoorientatorId == userContext.UserId));
                default:
                    return query.Where(d => false);
            }
        }
    }
}
