using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class DiscussionForum
{
    public int ForumId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime? LastActiveTimestamp { get; set; }
    
    public virtual ICollection<InstructorJoin> InstructorJoins { get; set; } = new List<InstructorJoin>(); // Add this line
    
    public int ModuleId { get; set; }

    public virtual ICollection<Join> Joins { get; set; } = new List<Join>();

    public virtual Module Module { get; set; } = null!;
}
