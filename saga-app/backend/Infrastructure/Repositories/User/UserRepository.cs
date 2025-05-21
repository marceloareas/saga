using saga.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace saga.Infrastructure.Repositories.User
{
    /// <inheritdoc />
    public class UserRepository : BaseRepository<UserEntity>, IUserRepository
    {
        public UserRepository(ContexRepository dbContext) : base(dbContext)
        {
        }

        /// <inheritdoc />
        public async Task<UserEntity?> GetUserByEmail(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
