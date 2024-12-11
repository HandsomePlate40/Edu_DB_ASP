using System.ComponentModel.DataAnnotations;

namespace Edu_DB_ASP.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; } // Role: Learner or Instructor

        // Additional fields for Learner or Instructor
        public string FullName { get; set; }
        public DateTime? BirthDate { get; set; } // Optional for instructors
        public string Qualifications { get; set; } // Only for instructors
    }


}
