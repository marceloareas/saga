using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using saga.Models.Enums;

namespace saga.Models.Entities
{
    /// <summary>
    /// Represents a student entity in the system.
    /// </summary>
    [Table("Students")]
    public record StudentEntity : BaseEntity
    {
        /// <summary>
        /// The ID of the user associated with this student entity.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The registration number of the student.
        /// </summary>
        public string Registration { get; set; }

        /// <summary>
        /// The date on which the student registered.
        /// </summary>
        public DateTime? RegistrationDate { get; set; }

        /// <summary>
        /// The ID of the project associated with this student entity.
        /// </summary>
        public Guid? ProjectId { get; set; }

        /// <summary>
        /// The status of the student.
        /// </summary>
        public StatusEnum Status { get; set; }

        /// <summary>
        /// The date on which the student entered the program.
        /// </summary>
        public DateTime? EntryDate { get; set; }

        /// <summary>
        /// The date on which the student defended their project.
        /// </summary>
        public DateTime? ProjectDefenceDate { get; set; }

        /// <summary>
        /// The date on which the student qualified for their project.
        /// </summary>
        public DateTime? ProjectQualificationDate { get; set; }

        /// <summary>
        /// The proficiency level of the student.
        /// </summary>
        public bool Proficiency { get; set; }

        /// <summary>
        /// The CPF (taxpayer ID) of the student.
        /// </summary>
        public string? CPF { get; set; }

        /// <summary>
        /// The name of the undergraduate institution the student attended.
        /// </summary>
        public string? UndergraduateInstitution { get; set; }

        /// <summary>
        /// The type of institution the student attended.
        /// </summary>
        public InstitutionTypeEnum InstitutionType { get; set; }

        /// <summary>
        /// The name of the undergraduate course the student completed.
        /// </summary>
        public string? UndergraduateCourse { get; set; }

        /// <summary>
        /// The year in which the student graduated from their undergraduate program.
        /// </summary>
        public int GraduationYear { get; set; }

        /// <summary>
        /// The academic area in which the student completed their undergraduate program.
        /// </summary>
        public UndergraduateAreaEnum UndergraduateArea { get; set; }

        /// <summary>
        /// The date of birth of the student.
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// The scholarship status of the student.
        /// </summary>
        public ScholarshipEnum Scholarship { get; set; }

        /// <summary>
        /// The date on which the student qualified for their project.
        /// </summary>
        public DateTime? LastNotification { get; set; }

        /// <summary>
        /// Gets or sets the navigation property for the associated user entity.
        /// </summary>
        /// <remarks>
        /// This property allows lazy loading of the associated <see cref="UserEntity"/> entity.
        /// </remarks>
        public virtual UserEntity? User { get; set; }

        /// <summary>
        /// Gets or sets the navigation property for the associated project entity.
        /// </summary>
        /// <remarks>
        /// This property allows lazy loading of the associated <see cref="ProjectEntity"/> entity.
        /// </remarks>
        public virtual ProjectEntity? Project { get; set; }

        /// <summary>
        /// Gets or sets the navigation property for the collection of student course entities associated with this student.
        /// </summary>
        /// <remarks>
        /// This property allows lazy loading of the associated collection of <see cref="StudentCourseEntity"/> entities.
        /// </remarks>
        public virtual IEnumerable<StudentCourseEntity> StudentCourses { get; set; }

        /// <summary>
        /// Gets or sets the navigation property for the orientations entity associated with this student.
        /// </summary>
        /// <remarks>
        /// This property allows lazy loading of the associated <see cref="OrientationEntity"/> entity.
        /// </remarks>
        public virtual OrientationEntity? Orientation { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentEntity"/> class with the specified properties.
        /// </summary>
        public StudentEntity()
        {
            StudentCourses = new List<StudentCourseEntity>();
        }
    }
}
