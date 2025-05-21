using System.ComponentModel;
using CsvHelper.Configuration.Attributes;

namespace saga.Models.Enums
{
    public enum UndergraduateAreaEnum
    {
        Default,
        [Name("Computação")]
        COMPUTATION,

        [Name("Exatas")]
        EXACT_SCIENCES,

        [Name("Humanas")]
        HUMANITIES,

        [Name("Saúde")]
        HEALTH,

        [Name("Engenharia")]
        ENGINEERING
    }
}
