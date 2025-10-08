using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using saga.Models.DTOs;
using saga.Models.DTOs.Common;
using saga.Services.Interfaces;

namespace saga.Controllers
{
    [ApiController]
    [Route("students")]
    [AllowAnonymous]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // Build a ProblemDetails manually so we can include traceId on older ASP.NET Core
        private ObjectResult Problem400(string title, string? detail = null)
        {
            var pd = new ProblemDetails
            {
                Type = "https://httpstatuses.io/400",
                Title = title,
                Detail = detail,
                Status = StatusCodes.Status400BadRequest,
                Instance = HttpContext?.Request?.Path.Value
            };

            // Add correlation id
            pd.Extensions["traceId"] = HttpContext?.TraceIdentifier ?? string.Empty;

            var result = new ObjectResult(pd)
            {
                StatusCode = pd.Status
            };
            result.ContentTypes.Add("application/problem+json");
            return result;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(StudentInfoDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<StudentInfoDto>> CreateStudent([FromBody] StudentDto studentDto)
        {
            try
            {
                var student = await _studentService.CreateStudentAsync(studentDto);
                return CreatedAtAction(nameof(GetStudent), new { studentId = student.Id }, student);
            }
            catch (ArgumentException ex)
            {
                return Problem400("Invalid student payload", ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Problem400("Business rule violation", ex.Message);
            }
        }

        [HttpPost("csv")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(IEnumerable<StudentInfoDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<StudentInfoDto>>> AddStudentsFromCsvAsync(IFormFile file)
        {
            try
            {
                var students = await _studentService.AddStudentsFromCsvAsync(file);
                return Ok(students);
            }
            catch (ArgumentException ex)
            {
                return Problem400("Invalid CSV", ex.Message);
            }
        }

        [HttpPost("course/csv")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(IEnumerable<StudentInfoDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<StudentInfoDto>>> AddCoursesToStudentsFromCsvAsync(IFormFile file)
        {
            try
            {
                var courses = await _studentService.AddCoursesToStudentsFromCsvAsync(file);
                return Ok(courses);
            }
            catch (ArgumentException ex)
            {
                return Problem400("Invalid CSV", ex.Message);
            }
        }

        [HttpGet("{studentId:guid}")]
        [Authorize(Roles = "Administrator, Professor, Student")]
        [ProducesResponseType(typeof(StudentInfoDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<StudentInfoDto>> GetStudent(Guid studentId)
        {
            var student = await _studentService.GetStudentAsync(studentId);
            if (student is null)
            {
                var pd = new ProblemDetails
                {
                    Type = "https://httpstatuses.io/404",
                    Title = "Student not found",
                    Status = StatusCodes.Status404NotFound,
                    Instance = HttpContext?.Request?.Path.Value
                };
                pd.Extensions["traceId"] = HttpContext?.TraceIdentifier ?? string.Empty;

                var res = new ObjectResult(pd) { StatusCode = pd.Status };
                res.ContentTypes.Add("application/problem+json");
                return res;
            }

            return Ok(student);
        }

        [HttpPut("{studentId}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(StudentInfoDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<StudentInfoDto>> UpdateStudent( Guid studentId, [FromBody] StudentDto studentDto)
        {
            try
            {
                var updated = await _studentService.UpdateStudentAsync(studentId, studentDto);
                if (updated == null)
                {
                    var pd = new ProblemDetails
                    {
                        Type = "https://httpstatuses.io/404",
                        Title = "Student not found",
                        Status = StatusCodes.Status404NotFound,
                        Instance = HttpContext?.Request?.Path.Value
                    };
                    pd.Extensions["traceId"] = HttpContext?.TraceIdentifier ?? string.Empty;

                    var res = new ObjectResult(pd) { StatusCode = pd.Status };
                    res.ContentTypes.Add("application/problem+json");
                    return res;
                }

                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return Problem400("Invalid student payload", ex.Message);
            }
        }

        [HttpDelete("{studentId}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteStudent(Guid studentId)
        {
            var studentDto = await _studentService.GetStudentAsync(studentId);
            if (studentDto == null)
            {
                var pd = new ProblemDetails
                {
                    Type = "https://httpstatuses.io/404",
                    Title = "Student not found",
                    Status = StatusCodes.Status404NotFound,
                    Instance = HttpContext?.Request?.Path.Value
                };
                pd.Extensions["traceId"] = HttpContext?.TraceIdentifier ?? string.Empty;

                var res = new ObjectResult(pd) { StatusCode = pd.Status };
                res.ContentTypes.Add("application/problem+json");
                return res;
            }

            await _studentService.DeleteStudentAsync(studentId);
            return NoContent();
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, Professor, Student")]
        [ProducesResponseType(typeof(IEnumerable<StudentInfoDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<StudentInfoDto>>> GetAllStudentsAsync()
        {
            var studentDtos = await _studentService.GetAllStudentsAsync();
            return Ok(studentDtos);
        }

        [HttpGet("paged")]
        [ProducesResponseType(typeof(PagedResult<StudentInfoDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<StudentInfoDto>>> GetStudentsPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50,
            [FromQuery] string? q = null)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 50;
            if (pageSize > 200) pageSize = 200;

            var result = await _studentService.GetStudentsPageAsync(page, pageSize, q);
            return Ok(result);
        }
    }
}
