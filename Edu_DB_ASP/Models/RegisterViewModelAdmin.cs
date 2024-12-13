using System.ComponentModel.DataAnnotations;

namespace Edu_DB_ASP.Models
{
    public class RegisterViewModelAdmin
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
       
    }
}
