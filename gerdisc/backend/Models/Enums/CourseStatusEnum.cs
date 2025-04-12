using CsvHelper.Configuration.Attributes;

namespace saga.Models.Enums
{
    public enum CourseStatusEnum
    {
        [Name("Sem Situação Definida")]
        Default,
        [Name("Aprovado")]
        Approved,
        [Name("Reprovado")]
        Repproved,
        [Name("Reprovado por Frequência")]
        RepprovedByFrequency,
        [Name("Aproveitamento de Créditos")]
        UseOfCredits
    }
}
