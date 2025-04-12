using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using saga.Infrastructure.Validations;

namespace saga.Models.DTOs
{
    public class ResetPasswordDto
    {
        public string? Password { get; set; }
    }
}
