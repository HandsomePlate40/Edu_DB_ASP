using System.ComponentModel.DataAnnotations;

namespace Edu_DB_ASP.Models
{
    
    public class AssessmentUpdateModel
    {
        [Required]
        public string title { get; set; }

        [Required]
        public int score { get; set; }

        [Required]
        public int LearnerId { get; set; }




    }

}
