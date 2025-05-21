using CsvHelper.Configuration.Attributes;

namespace saga.Models.Enums
{
    public enum InstitutionTypeEnum
    {
        Default,
        [Name("PUBLICA")]
        Publica,
        [Name("PARTICULAR")]
        Particular,
        [Name("CEFET/RJ")]
        CEFET
    }
}
