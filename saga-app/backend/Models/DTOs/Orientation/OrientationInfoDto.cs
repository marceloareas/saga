using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace saga.Models.DTOs
{
    public class OrientationInfoDto : OrientationDto
    {
        public Guid Id { get; set; }

        public virtual UserDto? Coorientator { get; set; }

        public virtual UserDto? Student { get; set; }

        public virtual ProjectInfoDto? Project { get; set; }

        public virtual UserDto? Professor { get; set; }

        public OrientationInfoDto()
        {
        }
    }
}
