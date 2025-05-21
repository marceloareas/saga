using saga.Models.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace saga.Models.DTOs
{
    public class ProjectInfoDto
    {
        public Guid? Id { get; set; }
        public Guid ResearchLineId { get; set; }
        public string? Name { get; set; }
        public ProjectStatusEnum Status { get; set; }
        public IEnumerable<UserDto>? Professors { get; set; }
        public IEnumerable<StudentInfoDto>? Students { get; set; }
        public IEnumerable<OrientationInfoDto>? Orientations { get; set; }

        public ProjectInfoDto()
        {
            Professors = new List<UserDto>();
            Students = new List<StudentInfoDto>();
            Orientations = new List<OrientationInfoDto>();
        }
    }
}
