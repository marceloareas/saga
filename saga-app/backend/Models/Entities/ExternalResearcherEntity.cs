using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace saga.Models.Entities
{
    /// <summary>
    /// Represents an external researcher in the system.
    /// </summary>
    [Table("ExternalResearchers")]
    public record ExternalResearcherEntity : BaseEntity
    {
        /// <summary>
        /// The user ID of the external researcher.
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
        /// a string representing the name of the institution that the external researcher is affiliated with.
        /// </summary>
        public string? Institution { get; set; }
    }
}
