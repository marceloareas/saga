using System.ComponentModel.DataAnnotations;
using saga.Infrastructure.Validations;
using saga.Models.Enums;

namespace saga.Models.DTOs
{
    public class StudentDto : UserDto
    {
        public string? Registration { get; set; }

        public DateTime? RegistrationDate { get; set; }

        [Required(ErrorMessage = "ProjectId is required")]
        public Guid? ProjectId { get; set; }

        public StatusEnum Status { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime? ProjectDefenceDate { get; set; }

        public DateTime? ProjectQualificationDate { get; set; }

        public bool Proficiency { get; set; }

        public string? UndergraduateInstitution { get; set; }

        public InstitutionTypeEnum InstitutionType { get; set; }

        public string? UndergraduateCourse { get; set; }

        public int GraduationYear { get; set; }

        public UndergraduateAreaEnum UndergraduateArea { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public ScholarshipEnum Scholarship { get; set; }

        public IEnumerable<StudentCourseDto>? StudentCourses { get; set; }

        [ValidRolesEnum(RolesEnum.Student)]
        public override RolesEnum Role { get; set; }

        public StudentDto()
        {
            Role = Enums.RolesEnum.Student;
        }
    }
}
