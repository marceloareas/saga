using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace saga.Models.DTOs
{
    public class OrientationDto
    {
        public Guid? CoorientatorId { get; set; }

        public Guid StudentId { get; set; }

        public string? Dissertation { get; set; }

        public Guid ProjectId { get; set; }

        public Guid ProfessorId { get; set; }
    }
}
