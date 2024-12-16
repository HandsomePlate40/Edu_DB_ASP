namespace Edu_DB_ASP.Models
{
    public class LearningPathViewModel
    {
        public int LearnerID { get; set; }
        public int ProfileID { get; set; }
        public string CompletionStatus { get; set; }
        public string CustomContent { get; set; }
        public string AdaptiveRules { get; set; }
        public int GoalID { get; set; }
    }
}