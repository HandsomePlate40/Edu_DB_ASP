using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class ContentLibrary
{
    public int ContentId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? ContentType { get; set; }

    public string? ContentUrl { get; set; }

    public string? Keywords { get; set; }

    public string? DifficultyLevel { get; set; }

    public string? LearningObjectives { get; set; }

    public int CourseId { get; set; }

    public virtual Course Course { get; set; } = null!;
}
