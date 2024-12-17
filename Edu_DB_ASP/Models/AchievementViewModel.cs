using System.ComponentModel.DataAnnotations;

public class AchievementViewModel
{
    public int LearnerID { get; set; }
    public int BadgeID { get; set; }

    [Required]
    [StringLength(500)]
    public string AchievmentDescription { get; set; }

    [Required]
    [StringLength(50)]
    public string Type { get; set; }
}