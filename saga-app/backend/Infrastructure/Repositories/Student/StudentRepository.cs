using System.Linq.Expressions;
using saga.Infrastructure.Extensions;
using saga.Infrastructure.Providers;
using saga.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

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
                .AsNoTracking()
                .Where(e => !e.IsDeleted)
                .Include(s => s.User)
                .FilterByUserRoleOrNoop(_userContext)
                .FirstOrDefaultAsync(e => e.Id == id); // <<< Usa Student.Id
        }

        /// <inheritdoc />
        public override async Task<StudentEntity?> GetByIdAsync(
            Guid id,
            params Expression<Func<StudentEntity, object>>[] includeProperties)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(e => !e.IsDeleted)
                .IncludeMultiple(includeProperties)
                .FilterByUserRoleOrNoop(_userContext)
                .SingleOrDefaultAsync(p => p.Id == id); // <<< Usa Student.Id
        }

        /// <inheritdoc />
        public override async Task<IEnumerable<StudentEntity>> GetAllAsync(
            params Expression<Func<StudentEntity, object>>[] includeProperties)
        {
            IQueryable<StudentEntity> q = _dbSet
                .AsNoTracking()
                .Where(e => !e.IsDeleted)
                .IncludeMultiple(includeProperties);

            q = q.FilterByUserRoleOrNoop(_userContext);

            return await q.ToListAsync();
        }

        /// <inheritdoc />
        public override async Task DeactiveByIdAsync(Guid id)
        {
            // Desativar por Student.Id
            var entityToDelete = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
            if (entityToDelete == null)
                throw new ArgumentNullException(nameof(entityToDelete));

            entityToDelete.IsDeleted = true;
            await UpdateAsync(entityToDelete);
        }

        public async Task<(IReadOnlyList<StudentEntity> Items, int TotalCount)> GetPagedAsync(
            int page,
            int pageSize,
            string? q = null,
            params Expression<Func<StudentEntity, object>>[] includes
         )
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 50;

            IQueryable<StudentEntity> query = _dbSet
                .AsNoTracking()
                .Where(s => !s.IsDeleted)
                .FilterByUserRoleOrNoop(_userContext);

            if (!string.IsNullOrWhiteSpace(q))
            {
                var term = q.Trim().ToLower();
                query = query.Where(s =>
                    (s.User != null && s.User.LastName.ToLower().Contains(term)) ||
                    (s.Registration != null && s.Registration.ToLower().Contains(term)) ||
                    (s.User != null && s.User.Email.ToLower().Contains(term))
                );
            }

            foreach (var include in includes ?? Array.Empty<Expression<Func<StudentEntity, object>>>())
                query = query.Include(include);

            var total = await query.CountAsync();

            var items = await query
                .OrderBy(s => s.User!.LastName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }
    }
}
