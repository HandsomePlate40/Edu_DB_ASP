using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class Notification
{
    public int NotificationId { get; set; }

    public string? MessageBody { get; set; }

    public DateTime? Timestamp { get; set; }

    public string? UrgencyLevel { get; set; }

    public bool? ReadStatus { get; set; }

    public virtual ICollection<PersonalizationProfile> PersonalizationProfiles { get; set; } = new List<PersonalizationProfile>();

    public virtual ICollection<Learner> Learners { get; set; } = new List<Learner>();
}
