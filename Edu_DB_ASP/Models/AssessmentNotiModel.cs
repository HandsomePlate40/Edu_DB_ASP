using System.ComponentModel.DataAnnotations;

namespace Edu_DB_ASP.Models
{
    
    public class AssessmentNotiModel
    {
        [Required]
        public string Message { get; set; }

        [Required]
        public string UrgencyLevel { get; set; }

        [Required]
        public int LearnerId { get; set; }




    }

}
