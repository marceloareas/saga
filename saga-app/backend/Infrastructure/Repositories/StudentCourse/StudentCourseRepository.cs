using saga.Models.Entities;

namespace saga.Infrastructure.Repositories.StudentCourse
{
    /// <inheritdoc />
    public class StudentCourseRepository : BaseRepository<StudentCourseEntity>, IStudentCourseRepository
    {
        public StudentCourseRepository(ContexRepository dbContext) : base(dbContext)
        {
        }
    }
}
