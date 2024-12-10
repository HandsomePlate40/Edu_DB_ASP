using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class LeaderboardStudentsCourse
{
    public int CourseId { get; set; }

    public int LearnerId { get; set; }

    public int LeaderboardId { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Leaderboard Leaderboard { get; set; } = null!;

    public virtual Learner Learner { get; set; } = null!;
}
