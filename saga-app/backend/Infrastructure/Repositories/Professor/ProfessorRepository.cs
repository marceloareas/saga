using System.Linq.Expressions;
using saga.Infrastructure.Extensions;
using saga.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace saga.Infrastructure.Repositories.Professor
{
    /// <inheritdoc />
    public class ProfessorRepository : BaseRepository<ProfessorEntity>, IProfessorRepository
    {
        public ProfessorRepository(ContexRepository dbContext) : base(dbContext)
        {
        }

        /// <inheritdoc />
        public override async Task<ProfessorEntity?> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .Where(e => !e.IsDeleted)
                .FirstOrDefaultAsync(e => e.UserId == id);
        }

        /// <inheritdoc />
        public override async Task<ProfessorEntity?> GetByIdAsync(
            Guid id,
            params Expression<Func<ProfessorEntity, object>>[] includeProperties)
        {
            return await _dbSet
                .Where(e => !e.IsDeleted)
                .IncludeMultiple(includeProperties)
                .SingleOrDefaultAsync(p => p.UserId == id);
        }

        /// <inheritdoc />
        public override async Task DeactiveByIdAsync(Guid id)
        {
            ProfessorEntity? entityToDelete = await _dbSet.FirstAsync(x => x.UserId == id);
            if (entityToDelete == null)
                throw new ArgumentNullException(nameof(entityToDelete));

            entityToDelete.IsDeleted = true;
            await UpdateAsync(entityToDelete);
        }

        public async Task<(IReadOnlyList<ProfessorEntity> Items, int TotalCount)> GetPagedAsync(
             int page,
             int pageSize,
             string? q = null,
             params Expression<Func<ProfessorEntity, object>>[] includes
         )
         {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 50;

            IQueryable<ProfessorEntity> query = _dbSet.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(q))
            {
                var term = q.Trim().ToLower();
                query = query.Where(p =>
                    (p.User != null && p.User.LastName.ToLower().Contains(term)) ||
                    (p.User != null && p.User.Email.ToLower().Contains(term))
                );
             }

            foreach (var include in includes ?? Array.Empty<Expression<Func<ProfessorEntity, object>>>())
                query = query.Include(include);

            var total = await query.CountAsync();
            var items = await query
                .OrderBy(p => p.User!.LastName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);         
        }
     }
}
