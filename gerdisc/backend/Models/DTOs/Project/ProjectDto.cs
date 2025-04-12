using System.ComponentModel.DataAnnotations;
using saga.Models.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace saga.Models.DTOs
{
    public class ProjectDto
    {
        [Required]
        public Guid ResearchLineId { get; set; }
        public string? Name { get; set; }
        public ProjectStatusEnum Status { get; set; }
        public List<string> ProfessorIds { get; set; }
        public ProjectDto()
        {
            ProfessorIds = new List<string>();
        }
    }
}
