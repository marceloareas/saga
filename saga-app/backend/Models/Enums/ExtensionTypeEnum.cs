using CsvHelper.Configuration.Attributes;

namespace saga.Models.Enums
{
    public enum ExtensionTypeEnum
    {
        Default,
        [Name("Defesa")]
        Defence,
        [Name("Qualificação")]
        Qualification
    }
}
