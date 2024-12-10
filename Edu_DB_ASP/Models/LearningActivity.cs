using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class LearningActivity
{
    public int ActivityId { get; set; }

    public string ActivityType { get; set; } = null!;

    public decimal? MaxPoints { get; set; }

    public int ModuleId { get; set; }

    public virtual ICollection<ActivityInstruction> ActivityInstructions { get; set; } = new List<ActivityInstruction>();

    public virtual ICollection<Express> Expresses { get; set; } = new List<Express>();

    public virtual ICollection<InteractionLog> InteractionLogs { get; set; } = new List<InteractionLog>();

    public virtual Module Module { get; set; } = null!;
}
