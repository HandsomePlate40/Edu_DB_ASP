using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class Evaluate
{
    public int CourseId { get; set; }

    public int ModuleId { get; set; }

    public int AssessmentId { get; set; }

    public virtual Assessment Assessment { get; set; } = null!;

    public virtual Course Course { get; set; } = null!;

    public virtual Module Module { get; set; } = null!;
}
