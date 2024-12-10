using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class QuestReward
{
    public int QuestId { get; set; }

    public string Rewards { get; set; } = null!;

    public virtual Quest Quest { get; set; } = null!;
}
