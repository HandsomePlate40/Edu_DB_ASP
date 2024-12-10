using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class Survey
{
    public int SurveyId { get; set; }

    public string? Title { get; set; }

    public DateTime? ResponseTimestamp { get; set; }

    public virtual ICollection<SurveyQuestion> SurveyQuestions { get; set; } = new List<SurveyQuestion>();

    public virtual ICollection<CourseEnrollment> CourseEnrollments { get; set; } = new List<CourseEnrollment>();

    public virtual ICollection<Learner> Learners { get; set; } = new List<Learner>();
}
