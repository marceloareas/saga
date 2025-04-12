using System.ComponentModel.DataAnnotations.Schema;

namespace saga.Models.Entities
{
    /// <summary>
    /// Represents a course in the application.
    /// </summary>
    [Table("Courses")]
    public record CourseEntity : BaseEntity
    {
        /// <summary>
        /// The name of the course.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the course.
        /// </summary>
        public string CourseUnique { get; set; }

        /// <summary>
        /// The number of credits associated with the course.
        /// </summary>
        public int Credits { get; set; }

        /// <summary>
        /// The code assigned to the course.
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// Indicates whether the course is elective or not.
        /// </summary>
        public bool IsElective { get; set; }

        /// <summary>
        /// The concept assigned to the course.
        /// </summary>
        public string? Concept { get; set; }
    }
}
