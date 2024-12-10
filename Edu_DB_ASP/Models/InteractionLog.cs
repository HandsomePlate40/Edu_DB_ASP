using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class InteractionLog
{
    public int LogId { get; set; }

    public DateTime? Timestamp { get; set; }

    public string? ActionType { get; set; }

    public decimal? Duration { get; set; }

    public int ActivityId { get; set; }

    public int LearnerId { get; set; }

    public virtual LearningActivity Activity { get; set; } = null!;

    public virtual Learner Learner { get; set; } = null!;
}
