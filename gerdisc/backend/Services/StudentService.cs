using saga.Infrastructure.Repositories;
using saga.Models.DTOs;
using saga.Models.Entities;
using saga.Models.Mapper;
using CsvHelper;
using System.Globalization;
using saga.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace saga.Services
{
    public class StudentService : IStudentService
    {
        private readonly IRepository _repository;
        private readonly ILogger<StudentService> _logger;
        private readonly IUserService _userService;

        public StudentService(
            IRepository repository,
            ILogger<StudentService> logger,
            IUserService userService
        )
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        /// <inheritdoc />
        public async Task<StudentInfoDto> CreateStudentAsync(StudentDto studentDto)
        {
            var user = await _userService.CreateUserAsync(studentDto);
            var student = studentDto.ToEntity(user.Id);

            student = await _repository.Student.AddAsync(student);

            _logger.LogInformation($"Student {studentDto.Email} created successfully.");
            return student.ToInfoDto();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<StudentInfoDto>> AddStudentsFromCsvAsync(IFormFile file)
        {
            var insertedStudents = new List<StudentInfoDto>();
            await foreach (var record in CastFromCsvAsync<StudentCsvDto>(file))
            {
                try
                {
                    var insertedStudent = await CreateStudentAsync(record.ToDto());
                    insertedStudents.Add(insertedStudent);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex.Message);
                }
            }

            return insertedStudents;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<StudentCourseDto>> AddCoursesToStudentsFromCsvAsync(IFormFile file)
        {
            var insertedCourses = new List<StudentCourseDto>();

            var records = CastFromCsvAsync<StudentCourseCsvDto>(file);
            var courseNames = await records.Select(x => x.CourseUnique).ToListAsync();
            var courses = await _repository.Course.GetAllAsync(x => courseNames.Contains(x.CourseUnique));
            var courseDictionary = courses?.ToDictionary(x => x.CourseUnique, x => x.Id);

            var studentRegistrations = await records.Select(x => x.StudentRegistration).ToListAsync();
            var students = await _repository.Student.GetAllAsync(x => studentRegistrations.Contains(x.Registration));
            var studentDictionary = students?.ToDictionary(x => x.Registration, x => x.Id);

            await foreach (var record in records)
            {
                if (studentDictionary.TryGetValue(record.StudentRegistration, out var student) &&
                    courseDictionary.TryGetValue(record.CourseUnique, out var courseId))
                {
                    var course = await _repository.StudentCourse.AddAsync(record.ToDto(courseId, student).ToEntity());
                    insertedCourses.Add(course.ToDto());
                }
                else
                {
                    _logger.LogWarning($"Failed to add course to student: {record.StudentRegistration} - {record.CourseName}");
                }
            }

            return insertedCourses;
        }

        /// <inheritdoc />
        public async Task<StudentInfoDto> GetStudentAsync(Guid id)
        {
            var studentEntity = await _repository.Student.GetByIdAsync(id, s => s.User, s => s.Project);
            if (studentEntity == null)
            {
                throw new ArgumentException("Student not found.");
            }

            return studentEntity.ToInfoDto();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<StudentInfoDto>> GetAllStudentsAsync()
        {
            var students = await _repository.Student.GetAllAsync(s => s.User);
            var studentDtos = students.Select(student => student.ToInfoDto());
            return studentDtos;
        }

        /// <inheritdoc />
        public async Task<StudentInfoDto> UpdateStudentAsync(Guid id, StudentDto studentDto)
        {
            var existingStudent = await GetExistingStudentAsync(id);
            existingStudent = studentDto.ToEntity(existingStudent);

            await _repository.Student.UpdateAsync(existingStudent);

            return existingStudent.ToInfoDto();
        }

        /// <inheritdoc />
        public async Task DeleteStudentAsync(Guid id)
        {
            var existingStudent = await GetExistingStudentAsync(id);
            await _repository.Student.DeactiveAsync(existingStudent);
        }

        private async Task<StudentEntity> GetExistingStudentAsync(Guid id)
        {
            var existingStudent = await _repository.Student.GetByIdAsync(id);
            if (existingStudent == null)
            {
                throw new ArgumentException($"Student with id {id} does not exist.");
            }
            return existingStudent;
        }

        private async IAsyncEnumerable<TDTO> CastFromCsvAsync<TDTO>(IFormFile file)
            where TDTO : class
        {
            using var reader = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = await csv.GetRecordsAsync<TDTO>().ToListAsync();

            foreach (var record in records)
            {
                if (TryValidateCsvRecord(record, out var errorMessages))
                {
                    yield return record;
                }
                else
                {
                    _logger.LogWarning($"Validation failed for record: {string.Join(", ", errorMessages)}");
                }
            }
        }

        private bool TryValidateCsvRecord(object record, out List<string> errorMessages)
        {
            var validationContext = new ValidationContext(record);
            var validationResults = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(record, validationContext, validationResults, true);
            errorMessages = isValid
                ? new List<string>()
                : validationResults.Select(result => result.ErrorMessage).ToList();

            return isValid;
        }
    }
}
