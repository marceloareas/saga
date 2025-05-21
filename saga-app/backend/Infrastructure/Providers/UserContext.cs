using saga.Models.Enums;

namespace saga.Infrastructure.Providers
{
    public class UserContext : IUserContext
    {
        /// <inheritdoc />
        public Guid? UserId { get; set; }

        /// <inheritdoc />
        public RolesEnum? Role { get; set; }
    }
}
