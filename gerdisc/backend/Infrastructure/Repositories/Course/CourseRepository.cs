using saga.Models.Entities;

namespace saga.Infrastructure.Repositories.Course
{
    /// <inheritdoc />
    public class CourseRepository : BaseRepository<CourseEntity>, ICourseRepository
    {
        public CourseRepository(ContexRepository dbContext) : base(dbContext)
        {
        }
    }
}
