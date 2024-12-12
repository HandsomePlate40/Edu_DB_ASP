using Edu_DB_ASP.Models;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;


namespace Edu_DB_ASP.Controllers.Account
{
    public class AccountController : Controller
    {
        private readonly EduDbContext _context;

        public AccountController(EduDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

     
       public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if(model.Role == "Learner"){

                    var Learner = _context.Learners.SingleOrDefault(u => u.Email == model.Email);

                    if (Learner != null && VerifyPassword(model.Password, Learner.PasswordHash))
                    {
                        // Create session or cookie
                        HttpContext.Session.SetString("UserEmail", Learner.Email);

                        return RedirectToAction("Index", "Home");
                    }

                }else if (model.Role == "Instructor")
                {

                    var Instructor = _context.Instructors.SingleOrDefault(u => u.Email == model.Email);

                    if (Instructor != null && VerifyPassword(model.Password, Instructor.PasswordHash))
                    {
                        // Create session or cookie
                        HttpContext.Session.SetString("UserEmail", Instructor.Email);

                        return RedirectToAction("Index", "Home");//redirect to login-registe page
                    }
                }

                ModelState.AddModelError("", "Invalid login attempt.");
            }

            return View(model);
        } 

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("Account/Register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.Role == "Learner")
                    {
                        var hashedPassword = HashPassword(model.Password);
                        var learner = new Learner
                        {
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Birthdate = model.BirthDate,
                            Gender = model.Gender,
                            CountryOfOrigin = model.CountryOfOrigin,
                            Email = model.Email,
                            PasswordHash = hashedPassword // Implement secure hashing
                        };
                        _context.Learners.Add(learner);
                        _context.SaveChanges();
                    }
                    else if (model.Role == "Instructor")
                    {
                        var hashedPassword = HashPassword(model.Password);
                        var instructor = new Instructor
                        {
                            InstructorName = $"{model.FirstName} {model.LastName}",
                            Qualifications = model.Qualifications,
                            Email = model.Email,
                            PasswordHash = hashedPassword // Implement secure hashing
                        };
                        _context.Instructors.Add(instructor);
                        _context.SaveChanges();
                    }

                    
                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                }
            }

            return View(model);
        }


        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            var hash = HashPassword(password);
            return hash == hashedPassword;
        }
    }
}
