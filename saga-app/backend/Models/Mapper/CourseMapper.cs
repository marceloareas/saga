using saga.Models.DTOs;
using saga.Models.Entities;

namespace saga.Models.Mapper
{
    /// <summary>
    /// A static class containing mapper methods for converting between <see cref="CourseDto"/> and <see cref="CourseEntity"/> objects.
    /// </summary>
    public static class CourseMapper
    {
        /// <summary>
        /// Converts a <see cref="CourseDto"/> object to a <see cref="CourseEntity"/> object.
        /// </summary>
        /// <param name="self">The <see cref="CourseDto"/> object to convert.</param>
        /// <returns>A new <see cref="CourseEntity"/> object with the values from the <paramref name="self"/> object.</returns>
        public static CourseEntity ToEntity(this CourseDto self) =>
            self is null ? new CourseEntity() : new CourseEntity
            {
                Code = self.Code,
                Name = self.Name,
                Concept = self.Concept,
                Credits = self.Credits,
                IsElective = self.IsElective,
                CourseUnique = self.CourseUnique
            };

        /// <summary>
        /// Updates the values of an existing <see cref="CourseEntity"/> object using the values from a <see cref="CourseDto"/> object.
        /// </summary>
        /// <param name="self">The <see cref="CourseDto"/> object containing the updated values.</param>
        /// <param name="entityToUpdate">The existing <see cref="CourseEntity"/> object to update.</param>
        /// <returns>The updated <see cref="CourseEntity"/> object.</returns>
        public static CourseEntity ToEntity(this CourseDto self, CourseEntity entityToUpdate)
        {
            entityToUpdate.Code = self.Code;
            entityToUpdate.Name = self.Name;
            entityToUpdate.Concept = self.Concept;
            entityToUpdate.Credits = self.Credits;
            entityToUpdate.IsElective = self.IsElective;
            entityToUpdate.CourseUnique = self.CourseUnique;
            return entityToUpdate;
        }

        /// <summary>
        /// Converts a <see cref="CourseEntity"/> object to a <see cref="CourseDto"/> object.
        /// </summary>
        /// <param name="self">The <see cref="CourseEntity"/> object to convert.</param>
        /// <returns>A new <see cref="CourseDto"/> object with the values from the <paramref name="self"/> object.</returns>
        public static CourseDto ToDto(this CourseEntity self) =>
            self is null ? new CourseDto() : new CourseDto
            {
                Id = self.Id,
                Code = self.Code,
                Name = self.Name,
                Concept = self.Concept,
                Credits = self.Credits,
                IsElective = self.IsElective,
                CourseUnique = self.CourseUnique
            };
    }
}
