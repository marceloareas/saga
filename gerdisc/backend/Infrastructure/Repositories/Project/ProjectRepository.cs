using saga.Infrastructure.Extensions;
using saga.Infrastructure.Providers;
using saga.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace saga.Infrastructure.Repositories.Project
{
    /// <inheritdoc />
    public class ProjectRepository : BaseRepository<ProjectEntity>, IProjectRepository
    {
        private readonly IUserContext _userContext;
        public ProjectRepository(ContexRepository dbContext, IUserContext userContext) : base(dbContext)
        {
            _userContext = userContext;
        }

        /// <inheritdoc />
        public override async Task<ProjectEntity?> GetByIdAsync(
            Guid id)
        {
            return await _dbSet
                .Where(e => !e.IsDeleted)
                .Include(x => x.ProfessorProjects.Where(pp => !pp.IsDeleted))
                .ThenInclude(x => x.Professor)
                .Include(x => x.Orientations.Where(o => !o.IsDeleted))
                .Include(x => x.Students.Where(s => !s.IsDeleted))
                .ThenInclude(x => x.User)
                .FilterByUserRole(_userContext)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <inheritdoc />
        public override async Task<IEnumerable<ProjectEntity>> GetAllAsync()
        {
            return await _dbSet
                .Where(e => !e.IsDeleted)
                .Include(x => x.ProfessorProjects.Where(pp => !pp.IsDeleted))
                .ThenInclude(x => x.Professor)
                .Include(x => x.Orientations.Where(o => !o.IsDeleted))
                .Include(x => x.Students.Where(s => !s.IsDeleted))
                .ThenInclude(x => x.User)
                .FilterByUserRole(_userContext)
                .ToListAsync();
        }
    }
}
