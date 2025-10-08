using System;
using System.Linq;
using saga.Infrastructure.Providers;
using saga.Models.Entities;
using saga.Models.Enums;

namespace saga.Infrastructure.Extensions
{
    public static class OrientationExtensions
    {
        public static IQueryable<OrientationEntity> FilterByUserRole(
            this IQueryable<OrientationEntity> query,
            IUserContext? userContext)
        {
            if (userContext is null) return query;

            switch (userContext.Role)
            {
                case RolesEnum.Professor:
                    // use logical OR (||), not bitwise |
                    return query.Where(d => d.ProfessorId == userContext.UserId || d.CoorientatorId == userContext.UserId);

                case RolesEnum.Student:
                    return userContext.UserId != Guid.Empty
                        ? query.Where(d => d.StudentId == userContext.UserId)
                        : query;

                case RolesEnum.Administrator:
                    return query;

                case RolesEnum.ExternalResearcher:
                    return userContext.UserId != Guid.Empty
                        ? query.Where(d => d.CoorientatorId == userContext.UserId)
                        : query;

                default:
                    // Fallback should NOT deny-all; keep it a no-op
                    return query;
            }
        }
    }
}
