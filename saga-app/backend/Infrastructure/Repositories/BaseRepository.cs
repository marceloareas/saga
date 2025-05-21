using System.Linq.Expressions;
using saga.Infrastructure.Extensions;
using saga.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace saga.Infrastructure.Repositories
{
    /// <summary>
    /// Represents the repository with CRUD operaton.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity that this repository works with.</typeparam>
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : BaseEntity
    {
        protected readonly ContexRepository _context;
        protected readonly DbSet<TEntity> _dbSet;

        public BaseRepository(ContexRepository context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet
                .Where(e => !e.IsDeleted)
                .ToListAsync();
        }

        /// <inheritdoc />
        public virtual async Task<TEntity?> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .Where(e => !e.IsDeleted)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        /// <inheritdoc />
        public virtual async Task<TEntity?> GetByIdAsync(
            Guid id,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return await _dbSet
                .Where(e => !e.IsDeleted)
                .IncludeMultiple(includeProperties)
                .SingleOrDefaultAsync(p => p.Id == id);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TEntity>> AddRangeAsync(
            IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return await _dbSet
                .Where(e => !e.IsDeleted)
                .IncludeMultiple(includeProperties)
                .ToListAsync();
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return await _dbSet
                .Where(e => !e.IsDeleted)
                .Where(predicate)
                .IncludeMultiple(includeProperties)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TEntity>> GetAllAsync(
            params Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _dbSet.Where(e => !e.IsDeleted);

            foreach (var includeProperty in includeProperties)
            {
                query = includeProperty(query);
            }

            return await query.ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TEntity>> GetPagedAsync(
            Expression<Func<TEntity, bool>> predicate,
            int pageNumber,
            int pageSize,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return await _dbSet
                .Where(e => !e.IsDeleted)
                .IncludeMultiple(includeProperties)
                .Where(predicate).Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<TEntity>> GetPagedAsync(
            int pageNumber,
            int pageSize)
        {
            return await _dbSet
                .Where(e => !e.IsDeleted)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet
                .Where(e => !e.IsDeleted)
                .Where(predicate)
                .ToListAsync();
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<TEntity>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet
                .Where(e => !e.IsDeleted)
                .Include(predicate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        /// <inheritdoc />
        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <inheritdoc />
        public virtual async Task UpdateAsync(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public virtual async Task DeactiveAsync(TEntity entity)
        {
            entity.IsDeleted = true;
            await UpdateAsync(entity);
        }

        /// <inheritdoc />
        public virtual async Task DeactiveByIdAsync(Guid id)
        {
            TEntity? entityToDelete = await _dbSet.FindAsync(id);
            if (entityToDelete == null)
                throw new ArgumentNullException(nameof(entityToDelete));

            entityToDelete.IsDeleted = true;
            await UpdateAsync(entityToDelete);
        }

        /// <inheritdoc />
        public virtual async Task DeactiveRangeAsync(
            IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
            }
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public virtual async Task DeactiveRangeAsync(
            Expression<Func<TEntity, bool>> predicate)
        {
            var entitiesToDelete = await _dbSet
                .Where(predicate)
                .ToListAsync();

            foreach (var entity in entitiesToDelete)
            {
                entity.IsDeleted = true;
            }
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public virtual async Task<int> CountAsync(
            Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet
                .AsNoTracking()
                .CountAsync(e => !e.IsDeleted && predicate.Compile()(e))
                .ConfigureAwait(false);
        }
    }
}
