using System.ComponentModel.DataAnnotations;
using saga.Infrastructure.Validations;
using saga.Models.Enums;
using System.Text.Json.Serialization;

namespace saga.Models.DTOs
{
    /// <summary>
    /// Represents a user cantract to the api.
    /// </summary>
    public class UserDto
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [ValidEmail(ErrorMessage = "Email is not in a valid format")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Cpf is required")]
        [ValidCpf]
        public string? Cpf { get; set; }
        public string? ResetPasswordPath { get; set; }
        public virtual RolesEnum Role { get; set; }
    }
}
