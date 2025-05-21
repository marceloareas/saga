using CsvHelper.Configuration.Attributes;

namespace saga.Models.Enums
{
    public enum ScholarshipEnum
    {
        Default,
        [Name("CEFET")]
        Cefet,
        [Name("CAPES")]
        Capes,
        [Name("FAPE/RJ")]
        FapeRj
    }
}
