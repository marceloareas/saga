using CsvHelper.Configuration.Attributes;
using saga.Infrastructure.Validations;
using saga.Models.Enums;

namespace saga.Models.DTOs
{
    public class StudentCsvDto
    {
        [Name("Nome")]
        public string? Name { get; set; }

        [Name("E-mail")]
        [ValidEmail]
        public string? Email { get; set; }

        [Name("CPF")]
        [ValidCpf]
        public string? Cpf { get; set; }

        [Name("Inscrição")]
        public string? Registration { get; set; }

        public string? RegistrationDate { get; set; }

        public string? ProjectId { get; set; }

        public StatusEnum Status { get; set; }

        public string? EntryDate { get; set; }

        public string? ProjectDefenceDate { get; set; }

        public string? ProjectQualificationDate { get; set; }

        [Optional]
        public string? Proficiency { get; set; }

        [Name("Instituição de Formação")]
        public string? UndergraduateInstitution { get; set; }

        public InstitutionTypeEnum InstitutionType { get; set; }

        [Name("Curso")]
        public string? UndergraduateCourse { get; set; }

        [Name("Ano de Formação")]
        public int GraduationYear { get; set; }

        [Optional]
        public UndergraduateAreaEnum UndergraduateArea { get; set; }

        [Name("Nascimento")]
        public string? DateOfBirth { get; set; }

        public ScholarshipEnum Scholarship { get; set; }
    }
}
