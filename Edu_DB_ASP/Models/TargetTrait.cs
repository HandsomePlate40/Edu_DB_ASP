using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class TargetTrait
{
    public int ModuleId { get; set; }

    public string Trait { get; set; } = null!;

    public virtual Module Module { get; set; } = null!;
}
