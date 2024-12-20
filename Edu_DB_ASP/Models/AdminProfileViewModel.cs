namespace Edu_DB_ASP.Models;

public class AdminProfileViewModel
{
    public Admin Admin { get; set; }
    public List<Learner> FirstFiveLearners { get; set; }
    public List<Instructor> FirstFiveInstructors { get; set; }
}