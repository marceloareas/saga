using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using saga.Infrastructure.Validations;
using saga.Models.Enums;

namespace saga.Models.DTOs
{
    public class ProfessorDto : UserDto
    {
        public string? Siape { get; set; }
        public List<string> ProjectIds { get; set; }

        [ValidRolesEnum(RolesEnum.Administrator, RolesEnum.Professor)]
        public override RolesEnum Role { get; set; }

        public ProfessorDto()
        {
            Role = Enums.RolesEnum.Professor;
            ProjectIds = new List<string>();
        }
    }
}
