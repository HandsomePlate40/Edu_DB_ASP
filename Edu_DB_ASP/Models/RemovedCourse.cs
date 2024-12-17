using System;

namespace Edu_DB_ASP.Models
{
    public class RemovedCourse
    {
        public int RemovedCourseId { get; set; }
        public int LearnerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}