using System.Linq.Expressions;
using saga.Infrastructure.Extensions;
using saga.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace saga.Infrastructure.Repositories.ExternalResearcher
{
    /// <inheritdoc />
    public class ExternalResearcherRepository : BaseRepository<ExternalResearcherEntity>, IExternalResearcherRepository
    {
        public ExternalResearcherRepository(ContexRepository dbContext) : base(dbContext)
        {
        }

        /// <inheritdoc />
        public override async Task<ExternalResearcherEntity?> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .Where(e => !e.IsDeleted)
                .FirstOrDefaultAsync(e => e.UserId == id);
        }

        /// <inheritdoc />
        public override async Task<ExternalResearcherEntity?> GetByIdAsync(
            Guid id,
            params Expression<Func<ExternalResearcherEntity, object>>[] includeProperties)
        {
            return await _dbSet
                .Where(e => !e.IsDeleted)
                .IncludeMultiple(includeProperties)
                .SingleOrDefaultAsync(p => p.UserId == id);
        }

        /// <inheritdoc />
        public override async Task DeactiveByIdAsync(Guid id)
        {
            ExternalResearcherEntity? entityToDelete = await _dbSet.FirstAsync(x => x.UserId == id);
            if (entityToDelete == null)
                throw new ArgumentNullException(nameof(entityToDelete));

            entityToDelete.IsDeleted = true;
            await UpdateAsync(entityToDelete);
        }
    }
}
