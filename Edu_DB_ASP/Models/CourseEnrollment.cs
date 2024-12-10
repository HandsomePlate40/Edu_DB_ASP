using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class CourseEnrollment
{
    public int EnrollmentId { get; set; }

    public int LearnerId { get; set; }

    public int CourseId { get; set; }

    public string? EnrollmentStatus { get; set; }

    public DateOnly? EnrollmentDate { get; set; }

    public DateOnly? CompletionDate { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Learner Learner { get; set; } = null!;

    public virtual ICollection<Survey> Surveys { get; set; } = new List<Survey>();
}
