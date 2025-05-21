namespace saga.Models.DTOs
{
    public class StudentCourseDto
    {
        public Guid StudentId { get; set; }

        public Guid CourseId { get; set; }

        public char Grade { get; set; }

        public int Year { get; set; }

        public int Trimester { get; set; }
    }
}
