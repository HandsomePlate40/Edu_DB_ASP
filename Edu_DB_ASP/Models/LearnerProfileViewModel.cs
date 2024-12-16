using Edu_DB_ASP.Models;

public class LearnerProfileViewModel
{
    public Learner Learner { get; set; }
    public List<EnrolledCourseViewModel> EnrolledCourses { get; set; }
    public List<DiscussionForum> AvailableForums { get; set; }
    public List<LearningGoal> LearningGoals { get; set; } // Add this property
}