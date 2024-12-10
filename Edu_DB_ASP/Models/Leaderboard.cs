using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class Leaderboard
{
    public int LeaderboardId { get; set; }

    public decimal TotalPoints { get; set; }

    public string? Season { get; set; }

    public virtual ICollection<LeaderboardRank> LeaderboardRanks { get; set; } = new List<LeaderboardRank>();

    public virtual ICollection<LeaderboardStudentsCourse> LeaderboardStudentsCourses { get; set; } = new List<LeaderboardStudentsCourse>();
}
