using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class LeaderboardRank
{
    public int LeaderboardId { get; set; }

    public int LeaderboardRank1 { get; set; }

    public int LearnerId { get; set; }

    public virtual Leaderboard Leaderboard { get; set; } = null!;

    public virtual Learner Learner { get; set; } = null!;
}
