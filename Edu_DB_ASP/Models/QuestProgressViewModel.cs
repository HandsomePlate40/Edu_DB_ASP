namespace Edu_DB_ASP.Models;

public class QuestProgressViewModel
{
    public int QuestID { get; set; }
    public string QuestTitle { get; set; }
    public string QuestDescription { get; set; }
    public string QuestStatus { get; set; }
    public int? BadgeID { get; set; }
    public string BadgeTitle { get; set; }
    public string BadgeDescription { get; set; }
    public string BadgeStatus { get; set; }
}