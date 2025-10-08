using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace saga.Infrastructure.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<TEntity> IncludeMultiple<TEntity>(
            this IQueryable<TEntity> query,
            params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class
        {
            if (includeProperties == null || includeProperties.Length == 0)
                return query;

            foreach (var includeProperty in includeProperties)
            {
                if (includeProperty is null) continue;
                query = query.Include(includeProperty);
            }

            return query;
        }
    }
}
