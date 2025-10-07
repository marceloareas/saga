using saga.Models.Entities;
using System.Linq.Expressions;

namespace saga.Infrastructure.Repositories.Professor
{
    /// <inheritdoc />
    public interface IProfessorRepository : IBaseRepository<ProfessorEntity>
    {
         Task<(IReadOnlyList<ProfessorEntity> Items, int TotalCount)> GetPagedAsync(
             int page,
             int pageSize,
             string? q = null,
             params Expression<Func<ProfessorEntity, object>>[] includes
         );
     }
}

