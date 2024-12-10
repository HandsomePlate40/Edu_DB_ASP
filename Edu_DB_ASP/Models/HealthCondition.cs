using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class HealthCondition
{
    public int CreationOrder { get; set; }

    public int LearnerId { get; set; }

    public string HealthCondition1 { get; set; } = null!;

    public virtual PersonalizationProfile PersonalizationProfile { get; set; } = null!;
}
