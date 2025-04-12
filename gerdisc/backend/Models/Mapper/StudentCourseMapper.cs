using saga.Models.DTOs;
using saga.Models.Entities;

namespace saga.Models.Mapper
{
    /// <summary>
    /// A static class containing mapper methods for converting between <see cref="StudentCourseDto"/> and <see cref="StudentCourseEntity"/> objects.
    /// </summary>
    public static class StudentCourseMapper
    {
        /// <summary>
        /// Converts a <see cref="StudentCourseDto"/> object to a <see cref="StudentCourseEntity"/> object.
        /// </summary>
        /// <param name="self">The <see cref="StudentCourseDto"/> object to convert.</param>
        /// <returns>A new <see cref="StudentCourseEntity"/> object with the values from the <paramref name="self"/> object.</returns>
        public static StudentCourseEntity ToEntity(this StudentCourseDto self) =>
            self is null ? new StudentCourseEntity() : new StudentCourseEntity
            {
                CourseId = self.CourseId,
                Grade = self.Grade,
                Trimester = self.Trimester,
                Year = self.Year
            };

        /// <summary>
        /// Updates the values of an existing <see cref="StudentCourseEntity"/> object using the values from a <see cref="StudentCourseDto"/> object.
        /// </summary>
        /// <param name="self">The <see cref="StudentCourseDto"/> object containing the updated values.</param>
        /// <param name="entityToUpdate">The existing <see cref="StudentCourseEntity"/> object to update.</param>
        /// <returns>The updated <see cref="StudentCourseEntity"/> object.</returns>
        public static StudentCourseEntity ToEntity(this StudentCourseDto self, StudentCourseEntity entityToUpdate)
        {
            entityToUpdate.CourseId = self.CourseId;
            entityToUpdate.Grade = self.Grade;
            entityToUpdate.Trimester = self.Trimester;
            entityToUpdate.Year = self.Year;
            return entityToUpdate;
        }

        /// <summary>
        /// Converts a <see cref="StudentCourseEntity"/> object to a <see cref="StudentCourseDto"/> object.
        /// </summary>
        /// <param name="self">The <see cref="StudentCourseEntity"/> object to convert.</param>
        /// <returns>A new <see cref="StudentCourseDto"/> object with the values from the <paramref name="self"/> object.</returns>
        public static StudentCourseDto ToDto(this StudentCourseEntity self) =>
            self is null ? new StudentCourseDto() : new StudentCourseDto
            {
                CourseId = self.CourseId,
                Grade = self.Grade,
                StudentId = self.StudentId,
                Trimester = self.Trimester,
                Year = self.Year
            };
    }
}
