using saga.Models.DTOs;
using saga.Models.Entities;

namespace saga.Models.Mapper
{
    /// <summary>
    /// A static class containing mapper methods for converting between <see cref="OrientationInfoDto"/> and <see cref="OrientationEntity"/> objects.
    /// </summary>
    public static class OrientationMapper
    {
        /// <summary>
        /// Converts a <see cref="OrientationDto"/> object to a <see cref="OrientationEntity"/> object.
        /// </summary>
        /// <param name="self">The <see cref="OrientationDto"/> object to convert.</param>
        /// <returns>A new <see cref="OrientationEntity"/> object with the values from the <paramref name="self"/> object.</returns>
        public static OrientationEntity ToEntity(this OrientationDto self) =>
            self is null ? new OrientationEntity() : new OrientationEntity
            {
                ProfessorId = self.ProfessorId,
                CoorientatorId = self.CoorientatorId,
                Dissertation = self.Dissertation,
                ProjectId = self.ProjectId,
                StudentId = self.StudentId
            };

        /// <summary>
        /// Updates the values of an existing <see cref="OrientationEntity"/> object using the values from a <see cref="OrientationDto"/> object.
        /// </summary>
        /// <param name="self">The <see cref="OrientationDto"/> object containing the updated values.</param>
        /// <param name="entityToUpdate">The existing <see cref="OrientationEntity"/> object to update.</param>
        /// <returns>The updated <see cref="OrientationEntity"/> object.</returns>
        public static OrientationEntity ToEntity(this OrientationDto self, OrientationEntity entityToUpdate)
        {
            entityToUpdate.CoorientatorId = self.CoorientatorId;
            entityToUpdate.ProfessorId = self.ProfessorId;
            entityToUpdate.Dissertation = self.Dissertation;
            entityToUpdate.ProjectId = self.ProjectId;
            entityToUpdate.StudentId = self.StudentId;
            return entityToUpdate;
        }

        /// <summary>
        /// Converts a <see cref="OrientationEntity"/> object to a <see cref="OrientationInfoDto"/> object.
        /// </summary>
        /// <param name="self">The <see cref="OrientationEntity"/> object to convert.</param>
        /// <returns>A new <see cref="OrientationInfoDto"/> object with the values from the <paramref name="self"/> object.</returns>
        public static OrientationInfoDto ToDto(this OrientationEntity self) =>
            self is null ? new OrientationInfoDto() : new OrientationInfoDto
            {
                Id = self.Id,
                Dissertation = self.Dissertation,
                CoorientatorId = self.CoorientatorId,
                ProfessorId = self.ProfessorId,
                ProjectId = self.ProjectId,
                StudentId = self.StudentId
            };

        /// <summary>
        /// Converts a <see cref="OrientationEntity"/> object to a <see cref="OrientationInfoDto"/> object.
        /// </summary>
        /// <param name="self">The <see cref="OrientationEntity"/> object to convert.</param>
        /// <returns>A new <see cref="OrientationInfoDto"/> object with the values from the <paramref name="self"/> object.</returns>
        public static OrientationInfoDto ToInfoDto(this OrientationEntity self) =>
            self is null ? new OrientationInfoDto() : new OrientationInfoDto
            {
                Id = self.Id,
                Dissertation = self.Dissertation,
                CoorientatorId = self.CoorientatorId,
                ProfessorId = self.ProfessorId,
                ProjectId = self.ProjectId,
                StudentId = self.StudentId,
                Coorientator = self.Coorientator?.ToUserDto(),
                Professor = self.Professor?.ToUserDto(),
                Student = self.Student?.ToUserDto()
            };
    }
}
