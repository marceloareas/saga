using saga.Infrastructure.Providers;
using saga.Models.Entities;
using saga.Models.Enums;

namespace saga.Infrastructure.Extensions
{
    public static class ExtensionRepositoryExtensions
    {
        public static IQueryable<ExtensionEntity> FilterByUserRole(this IQueryable<ExtensionEntity> query, IUserContext userContext)
        {
            switch (userContext.Role)
            {
                case RolesEnum.Student:
                    return query.Where(p => p.StudentId == userContext.UserId);
                case RolesEnum.Administrator:
                    return query;
                default:
                    return query.Where(d => false);
            }
        }
    }
}
