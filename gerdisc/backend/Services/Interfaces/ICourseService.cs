using System.Collections.Generic;
using System.Threading.Tasks;
using saga.Models.DTOs;

namespace saga.Services.Interfaces
{
    /// <summary>
    /// Interface for CourseService that defines the methods to manage courses.
    /// </summary>
    public interface ICourseService
    {
        /// <summary>
        /// Creates a new course.
        /// </summary>
        /// <param name="courseDto">The DTO object containing the course information.</param>
        /// <returns>The newly created course.</returns>
        Task<CourseDto> CreateCourseAsync(CourseDto courseDto);

        /// <summary>
        /// Retrieves a course by its ID.
        /// </summary>
        /// <param name="id">The ID of the course to retrieve.</param>
        /// <returns>The course matching the specified ID.</returns>
        Task<CourseDto> GetCourseAsync(Guid id);

        /// <summary>
        /// Retrieves a list of all courses.
        /// </summary>
        /// <returns>A list of all courses.</returns>
        Task<IEnumerable<CourseDto>> GetCoursesAsync();

        /// <summary>
        /// Updates a course.
        /// </summary>
        /// <param name="id">The ID of the course to update.</param>
        /// <param name="courseDto">The DTO object containing the updated course information.</param>
        /// <returns>The updated course.</returns>
        Task<CourseDto> UpdateCourseAsync(Guid id, CourseDto courseDto);

        /// <summary>
        /// Deletes a course.
        /// </summary>
        /// <param name="id">The ID of the course to delete.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task DeleteCourseAsync(Guid id);
    }
}
