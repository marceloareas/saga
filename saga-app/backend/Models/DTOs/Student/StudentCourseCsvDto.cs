using CsvHelper.Configuration.Attributes;
using saga.Models.Enums;

namespace saga.Models.DTOs
{
    public class StudentCourseCsvDto
    {
        [Name("MATR_ALUNO")]
        public string StudentRegistration { get; set; }

        [Name("NOME_DISCIPLINA")]
        public string CourseName { get; set; }

        [Name("CONCEITO")]
        public char Grade { get; set; }

        [Name("ANO")]
        public int Year { get; set; }

        [Name("PERIODO")]
        public string Trimester { get; set; }

        [Name("DISC")]
        public string CourseUnique { get; set; }

        [Name("SITUACAO")]
        public CourseStatusEnum Status { get; set; }
    }
}
