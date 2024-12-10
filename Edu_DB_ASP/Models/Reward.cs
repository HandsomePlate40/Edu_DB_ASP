using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class Reward
{
    public int RewardId { get; set; }

    public string? RewardType { get; set; }

    public decimal? RewardValue { get; set; }

    public string? RewardDescription { get; set; }

    public virtual ICollection<Quest> Quests { get; set; } = new List<Quest>();
}
