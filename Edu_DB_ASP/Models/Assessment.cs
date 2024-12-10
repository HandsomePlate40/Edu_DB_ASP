using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class Assessment
{
    public int AssessmentId { get; set; }

    public string? Title { get; set; }

    public string? AssessmentDescription { get; set; }

    public string? GradingCriteria { get; set; }

    public decimal? Weightage { get; set; }

    public decimal? MaxScore { get; set; }

    public int? TotalMarks { get; set; }

    public int? PassingMarks { get; set; }

    public int ModuleId { get; set; }

    public virtual ICollection<Evaluate> Evaluates { get; set; } = new List<Evaluate>();

    public virtual Module Module { get; set; } = null!;

    public virtual ICollection<TakenAssessment> TakenAssessments { get; set; } = new List<TakenAssessment>();
}
