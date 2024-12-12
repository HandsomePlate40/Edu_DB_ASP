using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class Instructor
{
    public string? InstructorName { get; set; }

    public string? Email { get; set; }

    public string? Qualifications { get; set; }

    public string PasswordHash { get; set; } = null!;

    public int InstructorId { get; set; }

    public virtual ICollection<Expertise> Expertises { get; set; } = new List<Expertise>();

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual ICollection<EmotionalFeedback> Feedbacks { get; set; } = new List<EmotionalFeedback>();

    public virtual ICollection<LearningPath> Paths { get; set; } = new List<LearningPath>();
}
