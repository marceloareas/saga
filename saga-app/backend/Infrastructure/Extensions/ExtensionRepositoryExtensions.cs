using System;
using System.Linq;
using saga.Infrastructure.Providers;
using saga.Models.Entities;
using saga.Models.Enums;

namespace saga.Infrastructure.Extensions
{
    public static class ExtensionRepositoryExtensions
    {
        public static IQueryable<ExtensionEntity> FilterByUserRole(
            this IQueryable<ExtensionEntity> query,
            IUserContext? userContext)
        {
            // No context/role → NO-OP (never Where(false))
            if (userContext is null) return query;

            switch (userContext.Role)
            {
                case RolesEnum.Student:
                    return userContext.UserId != Guid.Empty
                        ? query.Where(p => p.StudentId == userContext.UserId)
                        : query;

                case RolesEnum.Administrator:
                    return query;

                // Other roles currently see nothing special → NO-OP (not deny-all)
                default:
                    return query;
            }
        }
    }
}
