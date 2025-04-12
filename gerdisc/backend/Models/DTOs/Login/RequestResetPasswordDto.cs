using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using saga.Infrastructure.Validations;

namespace saga.Models.DTOs
{
    public class RequestResetPasswordDto
    {
        [Required(ErrorMessage = "Email is required")]
        [ValidEmail(ErrorMessage = "Email is not in a valid format")]
        public string? Email { get; set; }
        public string? ResetPasswordPath { get; set; }
    }
}
