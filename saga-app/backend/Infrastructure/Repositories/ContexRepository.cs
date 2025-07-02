using System.Linq.Expressions;
using saga.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace saga.Infrastructure.Repositories
{
    /// <summary>
    /// Represents the database context for the application, providing access to various entity sets.
    /// </summary>
    public class ContexRepository : DbContext
    {
        /// <summary>
        /// Gets the database URL.
        /// </summary>
        public string? DbUrl { get; }

        // Entity sets
        public DbSet<UserEntity> Users { get; set; } = null!;
        public DbSet<StudentEntity> Students { get; set; } = null!;
        public DbSet<ProfessorEntity> Professors { get; set; } = null!;
        public DbSet<CourseEntity> Courses { get; set; } = null!;
        public DbSet<ProjectEntity> Projects { get; set; } = null!;
        public DbSet<ExternalResearcherEntity> ExternalResearchers { get; set; } = null!;
        public DbSet<ExtensionEntity> Extensions { get; set; } = null!;
        public DbSet<ProfessorProjectEntity> ProfessorProjects { get; set; } = null!;
        public DbSet<StudentCourseEntity> StudentCourses { get; set; } = null!;
        public DbSet<OrientationEntity> Orientations { get; set; } = null!;
        public DbSet<ResearchLineEntity> ResearchLines { get; set; } = null!;
        public DbSet<PondocQualisEntity> Qualis { get; set; } = null!;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContexRepository"/> class with the specified options.
        /// </summary>
        /// <param name="options">The options for configuring the database context.</param>
        public ContexRepository(DbContextOptions<ContexRepository> options)
            : base(options)
        { }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Removed global filter for IsDeleted
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
