using saga.Models.Entities;

namespace saga.Infrastructure.Repositories.User
{
    /// <inheritdoc />
    public interface IUserRepository : IBaseRepository<UserEntity>
    {
        /// <summary>
        /// Gets a single User Entity from the repository by its email.
        /// </summary>
        /// <param name="email">The email of the entity to retrieve.</param>
        /// <returns>The User Entity with the specified email, or null if no such entity exists in the repository.</returns>
        Task<UserEntity?> GetUserByEmail(string email);
    }
}
