using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class EmotionalFeedback
{
    public int FeedbackId { get; set; }

    public int LearnerId { get; set; }

    public DateTime? Timestamp { get; set; }

    public string? EmotionalState { get; set; }

    public virtual ICollection<Express> Expresses { get; set; } = new List<Express>();

    public virtual Learner Learner { get; set; } = null!;

    public virtual ICollection<Instructor> Instructors { get; set; } = new List<Instructor>();
}
