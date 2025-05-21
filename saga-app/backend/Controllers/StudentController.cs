using Microsoft.AspNetCore.Mvc;
using saga.Models.DTOs;
using saga.Services;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<StudentInfoDto>> CreateStudent(StudentDto studentDto)
        {
            try
            {
                var student = await _studentService.CreateStudentAsync(studentDto);
                return CreatedAtAction(nameof(GetStudent), new { studentId = student.Id }, student);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("csv")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<IEnumerable<StudentInfoDto>>> AddStudentsFromCsvAsync(IFormFile file)
        {
            try
            {
                var students = await _studentService.AddStudentsFromCsvAsync(file);
                return Ok(students);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("course/csv")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<IEnumerable<StudentInfoDto>>> AddCoursesToStudentsFromCsvAsync(IFormFile file)
        {
            try
            {
                var courses = await _studentService.AddCoursesToStudentsFromCsvAsync(file);
                return Ok(courses);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{studentId}")]
        [Authorize(Roles = "Administrator, Professor, Student")]
        public async Task<ActionResult<StudentInfoDto>> GetStudent(Guid studentId)
        {
            try
            {
                var student = await _studentService.GetStudentAsync(studentId);
                return Ok(student);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{studentId}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<StudentInfoDto>> UpdateStudent(Guid studentId, StudentDto studentDto)
        {
            try
            {
                var updatedStudentDto = await _studentService.UpdateStudentAsync(studentId, studentDto);
                if (updatedStudentDto == null) return NotFound();
                return Ok(updatedStudentDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{studentId}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteStudent(Guid studentId)
        {
            try
            {
                var studentDto = await _studentService.GetStudentAsync(studentId);
                if (studentDto == null) return NotFound();
                await _studentService.DeleteStudentAsync(studentId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, Professor, Student")]
        [ProducesResponseType(typeof(IEnumerable<StudentInfoDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<StudentInfoDto>>> GetAllStudentsAsync()
        {
            try
            {
                var studentDtos = await _studentService.GetAllStudentsAsync();
                return Ok(studentDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
