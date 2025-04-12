using saga.Models.DTOs;
using saga.Models.Entities;

namespace saga.Models.Mapper
{
    /// <summary>
    /// A static class containing mapper methods for converting between <see cref="ResearchLineInfoDto"/> and <see cref="ResearchLineEntity"/> objects.
    /// </summary>
    public static class ResearchLineMapper
    {
        /// <summary>
        /// Converts a <see cref="ResearchLineDto"/> object to a <see cref="ResearchLineEntity"/> object.
        /// </summary>
        /// <param name="self">The <see cref="ResearchLineDto"/> object to convert.</param>
        /// <returns>A new <see cref="ResearchLineEntity"/> object with the values from the <paramref name="self"/> object.</returns>
        public static ResearchLineEntity ToEntity(this ResearchLineDto self) =>
            self is null ? new ResearchLineEntity() : new ResearchLineEntity
            {
                Name = self.Name,
            };

        /// <summary>
        /// Updates the values of an existing <see cref="ResearchLineEntity"/> object using the values from a <see cref="ResearchLineDto"/> object.
        /// </summary>
        /// <param name="self">The <see cref="ResearchLineDto"/> object containing the updated values.</param>
        /// <param name="entityToUpdate">The existing <see cref="ResearchLineEntity"/> object to update.</param>
        /// <returns>The updated <see cref="ResearchLineEntity"/> object.</returns>
        public static ResearchLineEntity ToEntity(this ResearchLineDto self, ResearchLineEntity entityToUpdate)
        {
            entityToUpdate.Name = self.Name;
            return entityToUpdate;
        }

        /// <summary>
        /// Converts a <see cref="ResearchLineEntity"/> object to a <see cref="ResearchLineInfoDto"/> object.
        /// </summary>
        /// <param name="self">The <see cref="ResearchLineEntity"/> object to convert.</param>
        /// <returns>A new <see cref="ResearchLineInfoDto"/> object with the values from the <paramref name="self"/> object.</returns>
        public static ResearchLineInfoDto ToDto(this ResearchLineEntity self) =>
            self is null ? new ResearchLineInfoDto() : new ResearchLineInfoDto
            {
                Id = self.Id,
                Name = self.Name,
                Projects = self.Projects.Select(p => p.ToInfoDto()).ToList(),
            };
    }
}
