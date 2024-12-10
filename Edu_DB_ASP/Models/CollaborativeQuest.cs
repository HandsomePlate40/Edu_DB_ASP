using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class CollaborativeQuest
{
    public int QuestId { get; set; }

    public int? MaxParticipants { get; set; }

    public DateTime? DeadLine { get; set; }

    public virtual Quest Quest { get; set; } = null!;
}
