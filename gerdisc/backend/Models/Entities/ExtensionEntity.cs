using System.ComponentModel.DataAnnotations.Schema;
using saga.Models.Enums;

namespace saga.Models.Entities
{
    /// <summary>
    /// Represents an extension in the system.
    /// </summary>
    [Table("Extensions")]
    public record ExtensionEntity : BaseEntity
    {
        /// <summary>
        /// Gets or sets the ID of the student associated with the extension.
        /// </summary>
        public Guid StudentId { get; set; }

        /// <summary>
        /// Gets or sets the number of days the extension is valid for.
        /// </summary>
        public int NumberOfDays { get; set; }

        /// <summary>
        /// Gets or sets the status of the extension.
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// Gets or sets the type of extension.
        /// </summary>
        public ExtensionTypeEnum Type { get; set; }

        /// <summary>
        /// Gets or sets the student associated with the extension.
        /// </summary>
        /// <remarks>
        /// This property is virtual to enable lazy loading of the associated Student entity
        /// by Entity Framework.
        /// </remarks>
        [ForeignKey("StudentId")]
        public virtual UserEntity? Student { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionEntity"/> class.
        /// </summary>
        public ExtensionEntity()
        {
            Student = null;
        }
    }
}
