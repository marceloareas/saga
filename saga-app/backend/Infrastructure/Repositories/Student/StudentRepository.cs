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

        public async Task<(IReadOnlyList<StudentEntity> Items, int TotalCount)> GetPagedAsync(
            int page,
            int pageSize,
            string? q = null,
            params Expression<Func<StudentEntity, object>>[] includes
         )
         {
             if (page < 1) page = 1;
             if (pageSize < 1) pageSize = 50;

             IQueryable<StudentEntity> query = _dbSet.AsNoTracking();

             // busca simples por nome/registro/email (ajuste os campos conforme sua entidade)
             if (!string.IsNullOrWhiteSpace(q))
             {
                 var term = q.Trim().ToLower();
                 query = query.Where(s =>
                     (s.User != null && s.User.Name.ToLower().Contains(term)) ||
                     (s.Registration != null && s.Registration.ToLower().Contains(term)) ||
                     (s.User != null && s.User.Email.ToLower().Contains(term))
                 );
             }

             foreach (var include in includes ?? Array.Empty<Expression<Func<StudentEntity, object>>>())
                 query = query.Include(include);

             var total = await query.CountAsync();
             var items = await query
                 .OrderBy(s => s.User!.Name) // ordenação previsível
                 .Skip((page - 1) * pageSize)
                 .Take(pageSize)
                 .ToListAsync();

             return (items, total);
         }
     }
}
