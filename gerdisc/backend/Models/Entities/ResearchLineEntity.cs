using System.ComponentModel.DataAnnotations.Schema;

namespace saga.Models.Entities
{
    /// <summary>
    /// Represents a researchLine in the system.
    /// </summary>
    [Table("ResearchLines")]
    public record ResearchLineEntity : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name of the researchLine.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the status of the researchLine.
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// Gets or sets the list of professors associated with the researchLine.
        /// </summary>
        public virtual IEnumerable<ProjectEntity> Projects { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResearchLineEntity"/> class.
        /// </summary>
        public ResearchLineEntity()
        {
            Projects = new List<ProjectEntity>();
        }
    }
}
