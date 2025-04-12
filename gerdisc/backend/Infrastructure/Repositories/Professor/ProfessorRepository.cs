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
    }
}
