using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class Badge
{
    public int BadgeId { get; set; }

    public string BadgeTitle { get; set; } = null!;

    public string? BadgeDescription { get; set; }

    public string? CriteriaToUnlock { get; set; }

    public decimal? PointsValue { get; set; }

    public virtual ICollection<Achievement> Achievements { get; set; } = new List<Achievement>();

    public virtual ICollection<Achievement> AchievementsNavigation { get; set; } = new List<Achievement>();

    public virtual ICollection<Learner> Learners { get; set; } = new List<Learner>();
}
