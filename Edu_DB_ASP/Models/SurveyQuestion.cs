using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class SurveyQuestion
{
    public int SurveyId { get; set; }

    public string Question { get; set; } = null!;

    public virtual Survey Survey { get; set; } = null!;
}
