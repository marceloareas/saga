using saga.Infrastructure.Validations;
using saga.Models.Enums;

namespace saga.Models.DTOs
{
    public class ProfessorInfoDto : UserDto
    {
        public string? Siape { get; set; }
        [ValidRolesEnum(RolesEnum.Administrator, RolesEnum.Professor)]
        public override RolesEnum Role { get; set; }

        public ProfessorInfoDto()
        {
            Role = Enums.RolesEnum.Professor;
        }
    }
}
