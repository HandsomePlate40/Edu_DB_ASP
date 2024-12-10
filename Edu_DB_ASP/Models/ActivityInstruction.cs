using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class ActivityInstruction
{
    public int ActivityId { get; set; }

    public string Instruction { get; set; } = null!;

    public virtual LearningActivity Activity { get; set; } = null!;
}
