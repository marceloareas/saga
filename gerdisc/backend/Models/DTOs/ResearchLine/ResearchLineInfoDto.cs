using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace saga.Models.DTOs
{
    public class ResearchLineInfoDto
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public List<ProjectInfoDto> Projects { get; set; }

        public ResearchLineInfoDto()
        {
            Projects = new List<ProjectInfoDto>();
        }
    }
}
