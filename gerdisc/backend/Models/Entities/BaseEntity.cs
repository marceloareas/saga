using System.ComponentModel.DataAnnotations;

namespace saga.Models.Entities
{
    /// <summary>
    /// Represents the base entity for all entities in the system.
    /// </summary>
    public record BaseEntity
    {
        /// <summary>
        /// Gets or sets the ID of the entity.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        public bool IsDeleted { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public BaseEntity()
        {
            IsDeleted = false;
        }
    }
}
