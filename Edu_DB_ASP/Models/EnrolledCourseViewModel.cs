using Edu_DB_ASP.Models;
namespace Edu_DB_ASP.Models;

public class EnrolledCourseViewModel
{
    public int CourseId { get; set; }
    public string Title { get; set; }
    public string CourseDescription { get; set; }
    public string DifficultyLevel { get; set; }
    public int CreditPoints { get; set; }
}