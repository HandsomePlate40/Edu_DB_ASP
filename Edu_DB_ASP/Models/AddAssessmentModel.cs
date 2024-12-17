using System.ComponentModel.DataAnnotations;

namespace Edu_DB_ASP.Models
{
    public class AddAssessmentModel
    {

        [Required]
        public string Title { get; set; }

        [Required]
        public string AssessmentDescription { get; set; }

        [Required]
        public string GradingCriteria { get; set; }

        [Required]
        public decimal Weightage { get; set; }

        [Required]
        public decimal MaxScore { get; set; }

        [Required]
        public int TotalMarks { get; set; }

        [Required]
        public int PassingMarks { get; set; }

        public int ModuleId { get; set; }


    }

}
