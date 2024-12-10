using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class LearningGoal
{
    public int GoalId { get; set; }

    public string? ObjectiveType { get; set; }

    public string? ProgressStatus { get; set; }

    public string? GoalDescription { get; set; }

    public DateTime? TimeBound { get; set; }

    public int? LearnerId { get; set; }

    public virtual Learner? Learner { get; set; }

    public virtual ICollection<LearningPath> LearningPaths { get; set; } = new List<LearningPath>();
}
