namespace Edu_DB_ASP.Models;

public class InstructorProfileViewModel
{
    public Instructor Instructor { get; set; }
    public List<Course> TaughtCourses { get; set; }
    public List<DiscussionForum> AvailableForums { get; set; }
}