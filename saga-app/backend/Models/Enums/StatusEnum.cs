using System.ComponentModel;
using CsvHelper.Configuration.Attributes;

namespace saga.Models.Enums
{
    public enum StatusEnum
    {
        Default,
        [Name("Ativo")]
        Active,
        [Name("Formado")]
        Graduated,
        [Name("Desligado")]
        Disconnected
    }
}
