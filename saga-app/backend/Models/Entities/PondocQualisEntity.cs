using System.ComponentModel.DataAnnotations.Schema;


namespace saga.Models.Entities;

[Table("Qualis")]
public record PondocQualisEntity : BaseEntity
{
    /// <summary>
    /// Periodical/conference document.
    /// </summary>
    public string? issn { get; set; }

    /// <summary>
    /// Periodical/conference name.
    /// </summary>
    public string? nome { get; set; }

    /// <summary>
    /// Brazilian periodical/conference classification.
    /// </summary>
    public string? qualis { get; set; }
}
