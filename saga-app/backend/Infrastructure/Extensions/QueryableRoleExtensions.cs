using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Query;
using saga.Infrastructure.Providers;
using saga.Models.Entities;

namespace saga.Infrastructure.Extensions
{
    public static class QueryableRoleExtensions
    {
        // Helper to normalize role to an upper-case string, regardless of original type
        private static string NormalizeRole(object? role)
        {
            // Handles string, enum (incl. nullable), or null
            return role switch
            {
                null => string.Empty,
                string s => s.Trim().ToUpperInvariant(),
                Enum e => e.ToString().Trim().ToUpperInvariant(),
                _ => role.ToString()?.Trim().ToUpperInvariant() ?? string.Empty
            };
        }

        // IQueryable<StudentEntity>
        public static IQueryable<StudentEntity> FilterByUserRoleOrNoop(
            this IQueryable<StudentEntity> source,
            IUserContext? ctx)
        {
            if (ctx is null) return source;

            // Works whether ctx.Role is string? or RolesEnum?
            var role = NormalizeRole(ctx.Role);

            if (string.IsNullOrEmpty(role))
                return source; // no restriction if role is empty

            // Admin: see all
            if (role == "ADMINISTRATOR")
                return source;

            // Student: restrict to own records when we have a valid user id
            if (role == "STUDENT" && ctx.UserId != Guid.Empty)
                return source.Where(s => s.UserId == ctx.UserId);

            // Professor: currently no extra restriction
            if (role == "PROFESSOR")
                return source;

            // Fallback: noop (avoid accidental WHERE FALSE)
            return source;
        }

        // IIncludableQueryable<StudentEntity, TProperty> overload
        public static IQueryable<StudentEntity> FilterByUserRoleOrNoop<TProperty>(
            this IIncludableQueryable<StudentEntity, TProperty> source,
            IUserContext? ctx)
        {
            return ((IQueryable<StudentEntity>)source).FilterByUserRoleOrNoop(ctx);
        }
    }
}
