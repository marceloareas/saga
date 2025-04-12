using saga.Models.Enums;

namespace saga.Infrastructure.Providers
{
    /// <summary>
    /// Represents the user context, providing information about the current user.
    /// </summary>
    public interface IUserContext
    {
        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        Guid? UserId { get; set; }

        /// <summary>
        /// Gets or sets the user role.
        /// </summary>
        RolesEnum? Role { get; set; }
    }
}
