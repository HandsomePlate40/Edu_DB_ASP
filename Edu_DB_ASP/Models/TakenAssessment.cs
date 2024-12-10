using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class TakenAssessment
{
    public int? ScoredPoints { get; set; }

    public int AssessmentId { get; set; }

    public int LearnerId { get; set; }

    public virtual Assessment Assessment { get; set; } = null!;

    public virtual Learner Learner { get; set; } = null!;
}
