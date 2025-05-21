using saga.Infrastructure.Repositories;
using saga.Models.DTOs;
using saga.Services;
using saga.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace saga.Controllers
{
    [ApiController]
    [Route("courses")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(CourseDto), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCourse(CourseDto courseDto)
        {
            try
            {
                var course = await _courseService.CreateCourseAsync(courseDto);
                return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, course);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator, Professor, Student")]
        [ProducesResponseType(typeof(CourseDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCourse(Guid id)
        {
            try
            {
                var course = await _courseService.GetCourseAsync(id);
                if (course == null)
                {
                    return NotFound();
                }
                return Ok(course);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CourseDto>), StatusCodes.Status200OK)]
        [Authorize(Roles = "Administrator, Professor, Student")]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetAllCoursesAsync()
        {
            var courseDtos = await _courseService.GetCoursesAsync();

            return Ok(courseDtos);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator, Professor")]
        [ProducesResponseType(typeof(CourseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCourse(Guid id, CourseDto courseDto)
        {
            try
            {
                var course = await _courseService.UpdateCourseAsync(id, courseDto);
                if (course == null)
                {
                    return NotFound();
                }
                return Ok(course);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator, Professor")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteCourseAsync(Guid id)
        {
            try
            {
                await _courseService.DeleteCourseAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
