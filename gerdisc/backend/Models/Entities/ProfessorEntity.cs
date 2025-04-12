using System.ComponentModel.DataAnnotations.Schema;

namespace saga.Models.Entities
{
    [Table("Professors")]
    public record ProfessorEntity : BaseEntity
    {
        /// <summary>
        /// The user ID of the professor.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property for the associated user entity.
        /// </summary>
        /// <remarks>
        /// This property allows lazy loading of the associated <see cref="UserEntity"/> entity.
        /// </remarks>
        public virtual UserEntity? User { get; set; }

        /// <summary>
        /// The SIAPE (System for Electronic Management of Educational Documentation) number of the professor.
        /// </summary>
        public string? Siape { get; set; }
    }
}
