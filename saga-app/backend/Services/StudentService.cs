using saga.Infrastructure.Repositories;
using saga.Models.DTOs;
using saga.Models.Entities;
using saga.Models.Mapper;
using CsvHelper;
using System.Globalization;
using saga.Services.Interfaces;
using System.ComponentModel.DataAnnotations;
using saga.Models.DTOs.Common;
using saga.Infrastructure.Validations;
using saga.Infrastructure.Exceptions;


namespace saga.Services
{
    public class StudentService : IStudentService
    {
        private readonly IRepository _repository;
        private readonly ILogger<StudentService> _logger;
        private readonly IUserService _userService;
        private readonly Validations _validations;

        public StudentService(
            IRepository repository,
            ILogger<StudentService> logger,
            IUserService userService,
            Validations validations
        )
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _validations = validations ?? throw new ArgumentNullException(nameof(validations));
        }

        /// <inheritdoc />
        public async Task<StudentInfoDto> CreateStudentAsync(StudentDto studentDto)
        {
            (var canCreate, var msgCreate) = await _validations.StudentValidator.CanCreate(studentDto);
            if (!canCreate) throw new ArgumentException(msgCreate);

            StudentEntity? student = null;
            await _repository.ExecuteInTransactionAsync(async () =>
            {
                var user = await _userService.CreateUserAsync(studentDto);
                student = await _repository.Student.AddAsync(studentDto.ToEntity(user.Id));
                await _repository.CommitAsync();
            });

            _logger.LogInformation("Student {Email} created successfully.", studentDto.Email);
            return student!.ToInfoDto();
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
                throw new NotFoundException($"Student with id {id} not found.");

            return studentEntity.ToInfoDto();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<StudentInfoDto>> GetAllStudentsAsync()
        {
            var students = await _repository.Student.GetAllAsync(s => s.User);
            var studentDtos = students.Select(student => student.ToInfoDto());
            return studentDtos;
        }

        public async Task<PagedResult<StudentInfoDto>> GetStudentsPageAsync(int page = 1, int pageSize = 50, string? q = null)
        {
            var (items, total) = await _repository.Student.GetPagedAsync(page, pageSize, q, s => s.User);
            return new PagedResult<StudentInfoDto>
            {
                Items = items.Select(s => s.ToInfoDto()).ToList(),
                Page = page,
                PageSize = pageSize,
                TotalCount = total
            };
        }

        /// <inheritdoc />
        public async Task<StudentInfoDto> UpdateStudentAsync(Guid id, StudentDto studentDto)
        {
            var existingStudent = await _repository
                .Student
                .GetByIdAsync(id, s => s.User) ?? throw new NotFoundException($"Student with id {id} not found.");
            
            (var canUpdate, var msgUpdate) = await _validations.StudentValidator.CanUpdate(studentDto, id);
            if (!canUpdate) throw new ArgumentException(msgUpdate);

            existingStudent = studentDto.ToEntity(existingStudent);
            
            await _repository.ExecuteInTransactionAsync(async () =>
            {
                await _userService.UpdateUserAsync(existingStudent.UserId, studentDto);
                await _repository.Student.UpdateAsync(existingStudent);
                await _repository.CommitAsync();
            });
            
            return existingStudent.ToInfoDto();
        }

        /// <inheritdoc />
        public async Task DeleteStudentAsync(Guid id)
        {
            var existingStudent = await GetExistingStudentAsync(id);
            await _repository.ExecuteInTransactionAsync(async () =>
            {
                await _repository.Student.DeactiveAsync(existingStudent);
                await _userService.DeleteUserAsync(existingStudent.UserId);
                await _repository.CommitAsync();
            });
        }

        private async Task<StudentEntity> GetExistingStudentAsync(Guid id)
        {
            var existingStudent = await _repository.Student.GetByIdAsync(id);
            if (existingStudent == null)
                throw new NotFoundException($"Student with id {id} not found.");
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
