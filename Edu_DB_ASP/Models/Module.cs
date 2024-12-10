using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class Module
{
    public int ModuleId { get; set; }

    public string Title { get; set; } = null!;

    public string? ContentType { get; set; }

    public string? ContentUrl { get; set; }

    public string? ModuleDifficulty { get; set; }

    public int CourseId { get; set; }

    public virtual ICollection<Assessment> Assessments { get; set; } = new List<Assessment>();

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<DiscussionForum> DiscussionForums { get; set; } = new List<DiscussionForum>();

    public virtual ICollection<Evaluate> Evaluates { get; set; } = new List<Evaluate>();

    public virtual ICollection<LearningActivity> LearningActivities { get; set; } = new List<LearningActivity>();

    public virtual ICollection<TargetTrait> TargetTraits { get; set; } = new List<TargetTrait>();
}
