using System.ComponentModel.DataAnnotations.Schema;
using saga.Models.Enums;

namespace saga.Models.Entities
{
    /// <summary>
    /// Represents a user in the Database.
    /// </summary>
    [Table("Users")]
    public record UserEntity : BaseEntity
    {
        /// <summary>
        /// The first name of the user.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// The last name of the user.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// The cpf of the user.
        /// </summary>
        public string? Cpf { get; set; }

        /// <summary>
        /// The email of the user.
        /// </summary>
        public string? Email { get; init; }

        /// <summary>
        /// The password hash of the user.
        /// </summary>
        public string? PasswordHash { get; set; }

        /// <summary>
        /// The role of the user.
        /// </summary>
        public RolesEnum Role { get; set; }

        /// <summary>
        /// The date and time when the user was created.
        /// </summary>
        public DateTime CreatedAt { get; init; }
    }
}
