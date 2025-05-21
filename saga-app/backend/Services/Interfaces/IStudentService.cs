using saga.Models.DTOs;
using saga.Models.Entities;

namespace saga.Services.Interfaces
{
    /// <summary>
    /// Provides CRUD operations for managing student entities.
    /// </summary>
    public interface IStudentService
    {
        /// <summary>
        /// Creates a new student entity.
        /// </summary>
        /// <param name="studentDto">The student entity to create.</param>
        /// <returns>The created student entity.</returns>
        Task<StudentInfoDto> CreateStudentAsync(StudentDto student);

        /// <summary>
        /// Adds a list of students from a CSV file asynchronously.
        /// </summary>
        /// <param name="file">The CSV file containing the student data.</param>
        /// <returns>An enumerable of the inserted students.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the input file is null.</exception>
        /// <exception cref="CsvHelper.CsvReaderException">Thrown when there is an error reading the CSV file.</exception>
        /// <exception cref="System.Exception">Thrown when there is an error creating the student in the database.</exception>
        Task<IEnumerable<StudentInfoDto>> AddStudentsFromCsvAsync(IFormFile file);

        /// <summary>
        /// Adds a list of courses to students from a CSV file asynchronously.
        /// </summary>
        /// <param name="file">The CSV file containing the courses data.</param>
        /// <returns>An enumerable of the inserted courses.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the input file is null.</exception>
        /// <exception cref="CsvHelper.CsvReaderException">Thrown when there is an error reading the CSV file.</exception>
        /// <exception cref="System.Exception">Thrown when there is an error creating the student in the database.</exception>
        Task<IEnumerable<StudentCourseDto>> AddCoursesToStudentsFromCsvAsync(IFormFile file);

        /// <summary>
        /// Gets a student entity by its ID.
        /// </summary>
        /// <param name="studentId">The ID of the student entity to retrieve.</param>
        /// <returns>The student entity with the specified ID.</returns>
        Task<StudentInfoDto> GetStudentAsync(Guid studentId);

        /// <summary>
        /// Update a student.
        /// </summary>
        /// <param name="id">The id of the student to update.</param>
        /// <param name="studentDto">The student DTO to update.</param>
        /// <returns>The updated student DTO.</returns>
        public Task<StudentInfoDto> UpdateStudentAsync(Guid id, StudentDto studentDto);

        /// <summary>
        /// Deletes a student entity.
        /// </summary>
        /// <param name="studentId">The ID of the student entity to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteStudentAsync(Guid studentId);

        /// <summary>
        /// Gets a list of all student entities.
        /// </summary>
        /// <returns>A list of all student entities.</returns>
        Task<IEnumerable<StudentInfoDto>> GetAllStudentsAsync();
    }
}
