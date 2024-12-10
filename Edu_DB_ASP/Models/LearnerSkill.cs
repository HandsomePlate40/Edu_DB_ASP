using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class LearnerSkill
{
    public int LearnerId { get; set; }

    public string Skill { get; set; } = null!;

    public virtual Learner Learner { get; set; } = null!;
}
