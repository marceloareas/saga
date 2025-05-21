using saga.Models.DTOs;
using saga.Models.Entities;
using saga.Models.Enums;

namespace saga.Models.Mapper
{
    /// <summary>
    /// A static class containing mapper methods for converting between <see cref="ExternalResearcherDto"/> and <see cref="ExternalResearcherEntity"/> objects.
    /// </summary>
    public static class ExternalResearcherMapper
    {
        /// <summary>
        /// Converts a <see cref="ExternalResearcherDto"/> object to a <see cref="ExternalResearcherEntity"/> object.
        /// </summary>
        /// <param name="self">The <see cref="ExternalResearcherDto"/> object to convert.</param>
        /// <returns>A new <see cref="ExternalResearcherEntity"/> object with the values from the <paramref name="self"/> object.</returns>
        public static ExternalResearcherEntity ToEntity(this ExternalResearcherDto self, Guid userId) =>
            self is null ? new ExternalResearcherEntity() : new ExternalResearcherEntity
            {
                Id = userId,
                Institution = self.Institution,
                UserId = userId,
            };

        /// <summary>
        /// Updates the values of an existing <see cref="ExternalResearcherEntity"/> object using the values from a <see cref="ExternalResearcherDto"/> object.
        /// </summary>
        /// <param name="self">The <see cref="ExternalResearcherDto"/> object containing the updated values.</param>
        /// <param name="entityToUpdate">The existing <see cref="ExternalResearcherEntity"/> object to update.</param>
        /// <returns>The updated <see cref="ExternalResearcherEntity"/> object.</returns>
        public static ExternalResearcherEntity ToEntity(this ExternalResearcherDto self, ExternalResearcherEntity entityToUpdate)
        {
            entityToUpdate.Institution = self.Institution;
            return entityToUpdate;
        }

        /// <summary>
        /// Converts a <see cref="ExternalResearcherEntity"/> object to a <see cref="ExternalResearcherDto"/> object.
        /// </summary>
        /// <param name="self">The <see cref="ExternalResearcherEntity"/> object to convert.</param>
        /// <returns>A new <see cref="ExternalResearcherDto"/> object with the values from the <paramref name="self"/> object.</returns>
        public static ExternalResearcherDto ToDto(this ExternalResearcherEntity self) =>
            self is null ? new ExternalResearcherDto() : new ExternalResearcherDto
            {
                Institution = self.Institution
            }.AddUserDto(self.User);
    }
}
