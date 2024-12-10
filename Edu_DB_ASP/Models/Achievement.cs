using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class Achievement
{
    public int AchievementId { get; set; }

    public string AchievementType { get; set; } = null!;

    public DateOnly? DateEarned { get; set; }

    public int LearnerId { get; set; }

    public int? BadgeId { get; set; }

    public string? AchievementDescription { get; set; }

    public virtual Badge? Badge { get; set; }

    public virtual Learner Learner { get; set; } = null!;

    public virtual ICollection<Badge> Badges { get; set; } = new List<Badge>();

    public virtual ICollection<Learner> Learners { get; set; } = new List<Learner>();
}
