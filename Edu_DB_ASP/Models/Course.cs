using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string Title { get; set; } = null!;

    public string? CourseDescription { get; set; }

    public string? DifficultyLevel { get; set; }

    public string? Prerequisites { get; set; }

    public decimal? CreditPoints { get; set; }

    public string? LearningObjectives { get; set; }

    public virtual ICollection<ContentLibrary> ContentLibraries { get; set; } = new List<ContentLibrary>();

    public virtual ICollection<CourseEnrollment> CourseEnrollments { get; set; } = new List<CourseEnrollment>();

    public virtual ICollection<Evaluate> Evaluates { get; set; } = new List<Evaluate>();

    public virtual ICollection<LeaderboardStudentsCourse> LeaderboardStudentsCourses { get; set; } = new List<LeaderboardStudentsCourse>();

    public virtual ICollection<Module> Modules { get; set; } = new List<Module>();

    public virtual ICollection<Instructor> Instructors { get; set; } = new List<Instructor>();
}
