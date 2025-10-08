using System;
using System.Linq;
using saga.Infrastructure.Providers;
using saga.Models.Entities;
using saga.Models.Enums;

namespace saga.Infrastructure.Extensions
{
    public static class StudentRepositoryExtensions
    {
        public static IQueryable<StudentEntity> FilterByUserRole(
            this IQueryable<StudentEntity> query,
            IUserContext? userContext)
        {
            if (userContext is null) return query;

            switch (userContext.Role)
            {
                case RolesEnum.Professor:
                    // Student has a Project where any orientation has this professor
                    return userContext.UserId != Guid.Empty
                        ? query.Where(p => p.Project != null && p.Project.Orientations.Any(x => x.ProfessorId == userContext.UserId))
                        : query;

                case RolesEnum.Student:
                    // Match by Student.UserId
                    return userContext.UserId != Guid.Empty
                        ? query.Where(p => p.UserId == userContext.UserId)
                        : query;

                case RolesEnum.Administrator:
                    return query;

                case RolesEnum.ExternalResearcher:
                    return userContext.UserId != Guid.Empty
                        ? query.Where(d => d.Project != null && d.Project.Orientations.Any(x => x.CoorientatorId == userContext.UserId))
                        : query;

                default:
                    // NO-OP fallback
                    return query;
            }
        }
    }
}
