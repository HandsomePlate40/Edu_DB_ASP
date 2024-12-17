using System.ComponentModel.DataAnnotations;

namespace Edu_DB_ASP.Models
{
    
    public class AssessmentUpdateModel
    {
        [Required]
        public int AssessmentId { get; set; }

        [Required]
        public int score { get; set; }

        [Required]
        public int LearnerId { get; set; }




    }

}
