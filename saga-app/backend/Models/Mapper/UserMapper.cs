using System.Text;
using saga.Models.DTOs;
using saga.Models.Entities;
using saga.Models.Enums;

namespace saga.Models.Mapper
{
    /// <summary>
    /// A static class containing mapper methods for converting between <see cref="UserDto"/> and <see cref="UserEntity"/> objects.
    /// </summary>
    public static class UserMapper
    {
        /// <summary>
        /// Converts a <see cref="UserDto"/> object to a <see cref="UserEntity"/> object.
        /// </summary>
        /// <param name="self">The <see cref="UserDto"/> object to convert.</param>
        /// <param name="role">The role to set for the <see cref="UserEntity"/> object.</param>
        /// <returns>A new <see cref="UserEntity"/> object with the values from the <paramref name="self"/> object.</returns>
        public static UserEntity ToUserEntity(this UserDto self) =>
            self is null ? new UserEntity() : new UserEntity
            {
                FirstName = self.FirstName,
                LastName = self.LastName,
                Cpf = self.Cpf,
                Email = self.Email.ToLower(),
                CreatedAt = DateTime.UtcNow,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(GeneratePassword(23)),
                Role = self.Role
            };

        /// <summary>
        /// Updates the values of an existing <see cref="UserEntity"/> object using the values from a <see cref="UserDto"/> object.
        /// </summary>
        /// <param name="self">The <see cref="UserDto"/> object containing the updated values.</param>
        /// <param name="entityToUpdate">The existing <see cref="UserEntity"/> object to update.</param>
        /// <returns>The updated <see cref="UserEntity"/> object.</returns>
        public static UserEntity ToUserEntity(this UserDto self, UserEntity entityToUpdate)
        {
            if (self is null)
            {
                return entityToUpdate;
            }
            entityToUpdate.FirstName = self.FirstName;
            entityToUpdate.LastName = self.LastName;
            entityToUpdate.Role = self.Role;
            return entityToUpdate;
        }

        /// <summary>
        /// Converts a <see cref="UserEntity"/> object to a <see cref="UserDto"/> object.
        /// </summary>
        /// <param name="self">The <see cref="UserEntity"/> object to convert.</param>
        /// <returns>A new <see cref="UserDto"/> object with the values from the <paramref name="self"/> object.</returns>
        public static UserDto ToUserDto(this UserEntity? self) =>
            self is null ? new UserDto() : new UserDto
            {
                Id = self.Id,
                FirstName = self.FirstName,
                LastName = self.LastName,
                Cpf = self.Cpf,
                Email = self.Email,
                Role = self.Role
            };

        /// <summary>
        /// Converts a <see cref="UserEntity"/> object to a <see cref="UserDto"/> object.
        /// </summary>
        /// <param name="self">The <see cref="UserEntity"/> object to convert.</param>
        /// <returns>A new <see cref="UserDto"/> object with the values from the <paramref name="self"/> object.</returns>
        public static TUserDto AddUserDto<TUserDto>(this TUserDto self, UserEntity? entity)
            where TUserDto : UserDto
        {
            if (entity is null)
            {
                return self;
            }
            self.Id = entity.Id;
            self.Cpf = entity.Cpf;
            self.Email = entity.Email;
            self.FirstName = entity.FirstName;
            self.LastName = entity.LastName;
            self.Role = entity.Role;
            return self;
        }

        /// <summary>
        /// Converts a <see cref="UserEntity"/> object to a <see cref="LoginResultDto"/> object.
        /// </summary>
        /// <param name="self">The <see cref="UserEntity"/> object to convert.</param>
        /// <returns>A new <see cref="LoginResultDto"/> object with the values from the <paramref name="self"/> object.</returns>
        public static LoginResultDto ToDto(this UserEntity user, string token) =>
            user is null ? new LoginResultDto() : new LoginResultDto
            {
                User = user.ToUserDto(),
                Token = token,
            };

        public static string GeneratePassword(int length)
        {
            var allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_-+=<>?";
            Random random = new();
            StringBuilder password = new();

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(allowedChars.Length);
                password.Append(allowedChars[index]);
            }

            return password.ToString();
        }

    }
}
