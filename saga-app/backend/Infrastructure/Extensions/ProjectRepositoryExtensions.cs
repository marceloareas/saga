using System;
using System.Linq;
using saga.Infrastructure.Providers;
using saga.Models.Entities;
using saga.Models.Enums;

namespace saga.Infrastructure.Extensions
{
    public static class ProjectRepositoryExtensions
    {
        public static IQueryable<ProjectEntity> FilterByUserRole(
            this IQueryable<ProjectEntity> query,
            IUserContext? userContext)
        {
            if (userContext is null) return query;

            switch (userContext.Role)
            {
                case RolesEnum.Professor:
                    // professor via ProfessorProjects OR Orientations
                    return userContext.UserId != Guid.Empty
                        ? query.Where(p =>
                              p.ProfessorProjects.Any(professor => professor.ProfessorId == userContext.UserId) ||
                              p.Orientations.Any(x => x.ProfessorId == userContext.UserId))
                        : query;

                case RolesEnum.Student:
                    // student projects: relate via Student.UserId (not Student.Id)
                    return userContext.UserId != Guid.Empty
                        ? query.Where(p => p.Students.Any(student => student.UserId == userContext.UserId))
                        : query;

                case RolesEnum.Administrator:
                    return query;

                case RolesEnum.ExternalResearcher:
                    return userContext.UserId != Guid.Empty
                        ? query.Where(p => p.Orientations.Any(x => x.CoorientatorId == userContext.UserId))
                        : query;

                default:
                    // NO-OP fallback (prevents WHERE FALSE)
                    return query;
            }
        }
    }
}
