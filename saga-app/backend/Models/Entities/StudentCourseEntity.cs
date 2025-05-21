using System.ComponentModel.DataAnnotations.Schema;
using saga.Models.Enums;

namespace saga.Models.Entities
{
    /// <summary>
    /// Represents the association between a student and a course they are enrolled in.
    /// </summary>
    [Table("StudentCourses")]
    public record StudentCourseEntity : BaseEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier of the student.
        /// </summary>
        /// <remarks>
        /// This property is a foreign key to the <see cref="StudentEntity"/> entity.
        /// </remarks>
        public Guid StudentId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the course.
        /// </summary>
        /// <remarks>
        /// This property is a foreign key to the <see cref="CourseEntity"/> entity.
        /// </remarks>
        public Guid CourseId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the course.
        /// </summary>
        /// <remarks>
        /// This property is a foreign key to the <see cref="CourseEntity"/> entity.
        /// </remarks>
        public virtual CourseEntity? Course { get; set; }

        /// <summary>
        /// Gets or sets the student navigation property.
        /// </summary>
        /// <remarks>
        /// This property allows lazy loading of the <see cref="StudentEntity"/> entity.
        /// </remarks>
        public virtual StudentEntity? Student { get; set; }

        /// <summary>
        /// Gets or sets the grade of the student in the course.
        /// </summary>
        public char Grade { get; set; }

        /// <summary>
        /// Gets or sets the year in which the student took the course.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets the trimester in which the student took the course.
        /// </summary>
        public int Trimester { get; set; }

        /// <summary>
        /// Gets or sets the status of the student in the course.
        /// </summary>
        public CourseStatusEnum Status { get; set; }
    }
}
