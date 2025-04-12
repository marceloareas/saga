using saga.Models.DTOs;
using saga.Models.Entities;

namespace saga.Models.Mapper
{
    /// <summary>
    /// A static class containing mapper methods for converting between <see cref="ProjectInfoDto"/> and <see cref="ProjectEntity"/> objects.
    /// </summary>
    public static class ProjectMapper
    {
        /// <summary>
        /// Converts a <see cref="ProjectDto"/> object to a <see cref="ProjectEntity"/> object.
        /// </summary>
        /// <param name="self">The <see cref="ProjectDto"/> object to convert.</param>
        /// <returns>A new <see cref="ProjectEntity"/> object with the values from the <paramref name="self"/> object.</returns>
        public static ProjectEntity ToEntity(this ProjectDto self) =>
            self is null ? new ProjectEntity() : new ProjectEntity
            {
                Name = self.Name,
                ResearchLineId = self.ResearchLineId,
                Status = self.Status
            };

        /// <summary>
        /// Updates the values of an existing <see cref="ProjectEntity"/> object using the values from a <see cref="ProjectDto"/> object.
        /// </summary>
        /// <param name="self">The <see cref="ProjectDto"/> object containing the updated values.</param>
        /// <param name="entityToUpdate">The existing <see cref="ProjectEntity"/> object to update.</param>
        /// <returns>The updated <see cref="ProjectEntity"/> object.</returns>
        public static ProjectEntity ToEntity(this ProjectDto self, ProjectEntity entityToUpdate)
        {
            entityToUpdate.Name = self.Name;
            entityToUpdate.ResearchLineId = self.ResearchLineId;
            entityToUpdate.Status = self.Status;
            return entityToUpdate;
        }

        /// <summary>
        /// Converts a <see cref="ProjectEntity"/> object to a <see cref="ProjectDto"/> object.
        /// </summary>
        /// <param name="self">The <see cref="ProjectEntity"/> object to convert.</param>
        /// <returns>A new <see cref="ProjectDto"/> object with the values from the <paramref name="self"/> object.</returns>
        public static ProjectDto ToDto(this ProjectEntity self) =>
            self is null ? new ProjectDto() : new ProjectDto
            {
                Name = self.Name,
                Status = self.Status,
                ResearchLineId = self.ResearchLineId
            };

        /// <summary>
        /// Converts a <see cref="ProjectEntity"/> object to a <see cref="ProjectInfoDto"/> object.
        /// </summary>
        /// <param name="self">The <see cref="ProjectEntity"/> object to convert.</param>
        /// <returns>A new <see cref="ProjectInfoDto"/> object with the values from the <paramref name="self"/> object.</returns>
        public static ProjectInfoDto ToInfoDto(this ProjectEntity self) =>
            self is null ? new ProjectInfoDto() : new ProjectInfoDto
            {
                Id = self.Id,
                Name = self.Name,
                Status = self.Status,
                ResearchLineId = self.ResearchLineId,
                Professors = self.ProfessorProjects?.Select(p => p.Professor.ToUserDto()),
                Students = self.Students?.Select(s => s.ToDto()),
                Orientations = self.Orientations?.Select(d => d.ToDto())
            };
    }
}
