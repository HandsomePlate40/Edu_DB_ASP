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
        public string Role { get; set; } // Learner or Instructor

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }


       // [Required]
        public string Gender { get; set; }

       // [Required]
        public string CountryOfOrigin { get; set; }

        // Optional for instructors
        public string Qualifications { get; set; }
    }


}


