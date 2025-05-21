using System.Linq.Expressions;
using saga.Infrastructure.Extensions;
using saga.Infrastructure.Providers;
using saga.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace saga.Infrastructure.Repositories.Extension
{
    /// <inheritdoc />
    public class ExtensionRepository : BaseRepository<ExtensionEntity>, IExtensionRepository
    {
        private readonly IUserContext _userContext;
        public ExtensionRepository(ContexRepository dbContext, IUserContext userContext) : base(dbContext)
        {
            _userContext = userContext;
        }

        /// <inheritdoc />
        public override async Task<ExtensionEntity?> GetByIdAsync(
            Guid id,
            params Expression<Func<ExtensionEntity, object>>[] includeProperties)
        {
            return await _dbSet
                .Where(e => !e.IsDeleted)
                .IncludeMultiple(includeProperties)
                .FilterByUserRole(_userContext)
                .SingleOrDefaultAsync(p => p.Id == id);
        }

        /// <inheritdoc />
        public override async Task<IEnumerable<ExtensionEntity>> GetAllAsync(
            params Expression<Func<ExtensionEntity, object>>[] includeProperties)
        {
            return await _dbSet
                .Where(e => !e.IsDeleted)
                .IncludeMultiple(includeProperties)
                .FilterByUserRole(_userContext)
                .ToListAsync();
        }
    }
}
