using System.ComponentModel.DataAnnotations.Schema;

namespace saga.Models.Entities
{
    /// <summary>
    /// Represents the orientation of a professor or external researcher in a student's dissertation project.
    /// </summary>
    [Table("Orientations")]
    public record OrientationEntity : BaseEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier of the professor.
        /// </summary>
        /// <remarks>
        /// This property is a foreign key to the <see cref="UserEntity"/> entity.
        /// </remarks>
        public Guid? CoorientatorId { get; set; }

        /// <summary>
        /// Gets or sets the professor navigation property.
        /// </summary>
        /// <remarks>
        /// This property allows lazy loading of the <see cref="UserEntity"/> entity.
        /// </remarks>
        [ForeignKey("CoorientatorId")]
        public virtual UserEntity? Coorientator { get; set; }

        /// <summary>
        /// Gets or sets the foreign key of the student that wrote the orientation.
        /// </summary>
        public Guid StudentId { get; set; }

        /// <summary>
        /// Name of the dissertation.
        /// </summary>
        public string? Dissertation { get; set; }

        /// <summary>
        /// Gets or sets the foreign key of the project associated with the orientation.
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property for the associated student entity.
        /// </summary>
        /// <remarks>
        /// This property allows lazy loading of the associated <see cref="UserEntity"/> entity.
        /// </remarks>
        [ForeignKey("StudentId")]
        public virtual UserEntity? Student { get; set; }

        /// <summary>
        /// Gets or sets the navigation property for the associated project entity.
        /// </summary>
        /// <remarks>
        /// This property allows lazy loading of the associated <see cref="ProjectEntity"/> entity.
        /// </remarks>
        public virtual ProjectEntity? Project { get; set; }

        /// <summary>
        /// Gets or sets the foreign key of the professor associated with the orientation.
        /// </summary>
        public Guid ProfessorId { get; set; }

        /// <summary>
        /// Gets or sets the project navigation property.
        /// </summary>
        /// <remarks>
        /// This property allows lazy loading of the <see cref="ProjectEntity"/> entity.
        /// </remarks>
        [ForeignKey("ProfessorId")]
        public virtual UserEntity? Professor { get; set; }
    }
}
