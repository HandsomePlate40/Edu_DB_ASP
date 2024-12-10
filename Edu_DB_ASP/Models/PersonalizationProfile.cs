using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class PersonalizationProfile
{
    public int CreationOrder { get; set; }

    public int LearnerId { get; set; }

    public string? PersonalityType { get; set; }

    public string? EmotionalState { get; set; }

    public string? AccessibilityPreferences { get; set; }

    public string? PreferredContentTypes { get; set; }

    public int? NotificationId { get; set; }

    public int? PathId { get; set; }

    public virtual ICollection<HealthCondition> HealthConditions { get; set; } = new List<HealthCondition>();

    public virtual Learner Learner { get; set; } = null!;

    public virtual Notification? Notification { get; set; }

    public virtual LearningPath? Path { get; set; }
}
