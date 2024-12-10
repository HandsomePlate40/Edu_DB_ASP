using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class SkillProgression
{
    public int ProgressId { get; set; }

    public string? SpecificSkill { get; set; }

    public string? ProficiencyLevel { get; set; }

    public int LearnerId { get; set; }

    public virtual Learner Learner { get; set; } = null!;
}
