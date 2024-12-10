using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class Join
{
    public int LearnerId { get; set; }

    public int ForumId { get; set; }

    public string? Post { get; set; }

    public virtual DiscussionForum Forum { get; set; } = null!;

    public virtual Learner Learner { get; set; } = null!;
}
