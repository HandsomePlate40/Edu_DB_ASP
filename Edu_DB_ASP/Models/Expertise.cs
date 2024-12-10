using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class Expertise
{
    public int InstructorId { get; set; }

    public string ExpertiseArea { get; set; } = null!;

    public virtual Instructor Instructor { get; set; } = null!;
}
