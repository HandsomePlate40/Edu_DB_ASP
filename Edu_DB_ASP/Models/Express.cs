using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class Express
{
    public int LearnerId { get; set; }

    public int ActivityId { get; set; }

    public int FeedbackId { get; set; }

    public virtual LearningActivity Activity { get; set; } = null!;

    public virtual EmotionalFeedback Feedback { get; set; } = null!;

    public virtual Learner Learner { get; set; } = null!;
}
