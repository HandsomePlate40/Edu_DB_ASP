using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class LearningPath
{
    public int PathId { get; set; }

    public string? CompletionStatus { get; set; }

    public string? LearningPathDescription { get; set; }

    public string? AdaptiveRules { get; set; }

    public int GoalId { get; set; }

    public int? CreationOrder { get; set; }

    public virtual LearningGoal Goal { get; set; } = null!;

    public virtual ICollection<PersonalizationProfile> PersonalizationProfiles { get; set; } = new List<PersonalizationProfile>();

    public virtual ICollection<Instructor> Instructors { get; set; } = new List<Instructor>();
}
