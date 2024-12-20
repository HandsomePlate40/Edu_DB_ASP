// File: Edu_DB_ASP/Models/InstructorJoin.cs
using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class InstructorJoin
{
    public int InstructorId { get; set; }

    public int ForumId { get; set; }

    public string? Post { get; set; }

    public virtual DiscussionForum Forum { get; set; } = null!;

    public virtual Instructor Instructor { get; set; } = null!;
}