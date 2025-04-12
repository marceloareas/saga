using System.ComponentModel.DataAnnotations;

namespace saga.Models.DTOs
{
    public record CourseDto
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }

        [Required]
        public string CourseUnique { get; set; }

        public int Credits { get; set; }

        public string? Code { get; set; }

        public bool IsElective { get; set; }

        public string? Concept { get; set; }
    }
}
