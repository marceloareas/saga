using saga.Models.Entities;
using System.Linq.Expressions;

namespace saga.Infrastructure.Repositories.Student
{
    /// <inheritdoc />
    public interface IStudentRepository : IBaseRepository<StudentEntity>
    {
         Task<(IReadOnlyList<StudentEntity> Items, int TotalCount)> GetPagedAsync(
             int page,
             int pageSize,
             string? q = null,
             params Expression<Func<StudentEntity, object>>[] includes
         );
    }

}
