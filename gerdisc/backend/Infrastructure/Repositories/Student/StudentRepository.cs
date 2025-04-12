using System.Linq.Expressions;
using saga.Infrastructure.Extensions;
using saga.Infrastructure.Providers;
using saga.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace saga.Infrastructure.Repositories.Student
{
    /// <inheritdoc />
    public class StudentRepository : BaseRepository<StudentEntity>, IStudentRepository
    {
        private readonly IUserContext _userContext;
        public StudentRepository(ContexRepository dbContext, IUserContext userContext) : base(dbContext)
        {
            _userContext = userContext;
        }

        /// <inheritdoc />
        public override async Task<StudentEntity?> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .Where(e => !e.IsDeleted)
                .FilterByUserRole(_userContext)
                .FirstOrDefaultAsync(e => e.UserId == id);
        }

        /// <inheritdoc />
        public override async Task<StudentEntity?> GetByIdAsync(
            Guid id,
            params Expression<Func<StudentEntity, object>>[] includeProperties)
        {
            return await _dbSet
                .Where(e => !e.IsDeleted)
                .IncludeMultiple(includeProperties)
                .FilterByUserRole(_userContext)
                .SingleOrDefaultAsync(p => p.UserId == id);
        }

        /// <inheritdoc />
        public override async Task<IEnumerable<StudentEntity>> GetAllAsync(
            params Expression<Func<StudentEntity, object>>[] includeProperties)
        {
            return await _dbSet
                .Where(e => !e.IsDeleted)
                .FilterByUserRole(_userContext)
                .IncludeMultiple(includeProperties)
                .ToListAsync();
        }

        /// <inheritdoc />
        public override async Task DeactiveByIdAsync(Guid id)
        {
            StudentEntity? entityToDelete = await _dbSet.FirstAsync(x => x.UserId == id);
            if (entityToDelete == null)
                throw new ArgumentNullException(nameof(entityToDelete));

            entityToDelete.IsDeleted = true;
            await UpdateAsync(entityToDelete);
        }
    }
}
