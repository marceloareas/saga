using saga.Models.DTOs;
using saga.Models.Entities;
using saga.Models.Enums;

namespace saga.Models.Mapper
{
    /// <summary>
    /// A static class containing mapper methods for converting between <see cref="ProfessorDto"/> and <see cref="ProfessorEntity"/> objects.
    /// </summary>
    public static class ProfessorMapper
    {
        /// <summary>
        /// Converts a <see cref="ProfessorDto"/> object to a <see cref="ProfessorEntity"/> object.
        /// </summary>
        /// <param name="self">The <see cref="ProfessorDto"/> object to convert.</param>
        /// <returns>A new <see cref="ProfessorEntity"/> object with the values from the <paramref name="self"/> object.</returns>
        public static ProfessorEntity ToEntity(this ProfessorDto self, Guid userId) =>
            self is null ? new ProfessorEntity() : new ProfessorEntity
            {
                Id = userId,
                Siape = self.Siape,
                UserId = userId,
            };

        /// <summary>
        /// Updates the values of an existing <see cref="ProfessorEntity"/> object using the values from a <see cref="ProfessorDto"/> object.
        /// </summary>
        /// <param name="self">The <see cref="ProfessorDto"/> object containing the updated values.</param>
        /// <param name="entityToUpdate">The existing <see cref="ProfessorEntity"/> object to update.</param>
        /// <returns>The updated <see cref="ProfessorEntity"/> object.</returns>
        public static ProfessorEntity ToEntity(this ProfessorDto self, ProfessorEntity entityToUpdate)
        {
            entityToUpdate.Siape = self.Siape;
            entityToUpdate.User = self.ToUserEntity(entityToUpdate.User);
            return entityToUpdate;
        }

        /// <summary>
        /// Converts a <see cref="ProfessorEntity"/> object to a <see cref="ProfessorInfoDto"/> object.
        /// </summary>
        /// <param name="self">The <see cref="ProfessorEntity"/> object to convert.</param>
        /// <returns>A new <see cref="ProfessorInfoDto"/> object with the values from the <paramref name="self"/> object.</returns>
        public static ProfessorInfoDto ToDto(this ProfessorEntity self) =>
            self is null ? new ProfessorInfoDto() : new ProfessorInfoDto
            {
                Siape = self.Siape,
            }.AddUserDto(self.User);
    }
}
