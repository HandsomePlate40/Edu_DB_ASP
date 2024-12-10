using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class SkillMasteryQuest
{
    public int QuestId { get; set; }

    public string? Skills { get; set; }

    public virtual Quest Quest { get; set; } = null!;
}
