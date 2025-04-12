using saga.Models.Enums;

namespace saga.Models.DTOs
{
    public class ExtensionInfoDto
    {
        public Guid? Id { get; set; }

        public Guid StudentId { get; set; }

        public int NumberOfDays { get; set; }

        public string? Status { get; set; }

        public ExtensionTypeEnum Type { get; set; }

        public UserDto? Student { get; set; }
    }
}
