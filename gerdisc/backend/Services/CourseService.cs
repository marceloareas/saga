using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using saga.Infrastructure.Repositories;
using saga.Models;
using saga.Models.DTOs;
using saga.Models.Mapper;
using saga.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace saga.Services
{
    public class CourseService : ICourseService
    {
        private readonly IRepository _repository;
        private readonly ILogger<CourseService> _logger;

        public CourseService(IRepository repository, ILogger<CourseService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public async Task<CourseDto> CreateCourseAsync(CourseDto courseDto)
        {
            var course = courseDto.ToEntity();

            await _repository.Course.AddAsync(course);

            _logger.LogInformation($"Course {course.Name} created successfully.");

            return course.ToDto();
        }

        /// <inheritdoc />
        public async Task<CourseDto> GetCourseAsync(Guid id)
        {
            var courseEntity = await _repository.Course.GetByIdAsync(id);
            if (courseEntity == null)
            {
                throw new ArgumentException($"Course with id {id} not found.");
            }

            return courseEntity.ToDto();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<CourseDto>> GetCoursesAsync()
        {
            var courses = await _repository.Course.GetAllAsync();
            var courseDtos = new List<CourseDto>();
            foreach (var course in courses)
            {
                courseDtos.Add(course.ToDto());
            }

            return courseDtos;
        }

        /// <inheritdoc />
        public async Task<CourseDto> UpdateCourseAsync(Guid id, CourseDto courseDto)
        {
            var existingCourse = await _repository.Course.GetByIdAsync(id);
            if (existingCourse == null)
            {
                throw new ArgumentException($"Course with id {id} does not exist.");
            }

            existingCourse = courseDto.ToEntity(existingCourse);

            await _repository.Course.UpdateAsync(existingCourse);

            return existingCourse.ToDto();
        }

        /// <inheritdoc />
        public async Task DeleteCourseAsync(Guid id)
        {
            var existingCourse = await _repository.Course.GetByIdAsync(id);
            if (existingCourse == null)
            {
                throw new ArgumentException($"Course with id {id} does not exist.");
            }

            await _repository.Course.DeactiveAsync(existingCourse);
        }
    }
}
