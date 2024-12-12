using Edu_DB_ASP.Models;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

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
        public IActionResult LearnerLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LearnerLogin(LoginViewModel model)
        {
            var learner = _context.Learners.SingleOrDefault(u => u.Email == model.Email);

            if (learner != null && VerifyPassword(model.Password, learner.PasswordHash))
            {
                // Create session or cookie
                HttpContext.Session.SetString("UserEmail", learner.Email);

                return RedirectToAction("Profile", "Account");
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }

        [HttpGet]
        public IActionResult InstructorLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult InstructorLogin(LoginViewModel model)
        {
            var instructor = _context.Instructors.SingleOrDefault(u => u.Email == model.Email);

            if (instructor != null && VerifyPassword(model.Password, instructor.PasswordHash))
            {
                // Create session or cookie
                HttpContext.Session.SetString("UserEmail", instructor.Email);

                return RedirectToAction("Profile", "Account");
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }

        [HttpGet]
        public IActionResult RegisterLearner()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterLearner(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var hashedPassword = HashPassword(model.Password);
                    var learner = new Learner
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Gender = model.Gender,
                        CountryOfOrigin = model.CountryOfOrigin,
                        Email = model.Email,
                        PasswordHash = hashedPassword
                    };
                    _context.Learners.Add(learner);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("LearnerLogin");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult RegisterInstructor()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterInstructor(RegisterViewModelInstructor model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var hashedPassword = HashPassword(model.Password);
                    var instructor = new Instructor
                    {
                        InstructorName = $"{model.FirstName} {model.LastName}",
                        Qualifications = model.Qualifications,
                        Email = model.Email,
                        PasswordHash = hashedPassword
                    };
                    _context.Instructors.Add(instructor);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("InstructorLogin");
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

        [HttpGet]
        public IActionResult GetStarted()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Profile()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (email == null)
            {
                return RedirectToAction("Login");
            }

            var learner = _context.Learners.SingleOrDefault(u => u.Email == email);
            if (learner == null)
            {
                return RedirectToAction("Login");
            }

            return View(learner);
        }

        [HttpPost]
        public async Task<IActionResult> UploadProfilePicture(IFormFile profilePicture)
        {
            if (profilePicture != null && profilePicture.Length > 0)
            {
                var email = HttpContext.Session.GetString("UserEmail");
                if (email == null)
                {
                    return RedirectToAction("Login");
                }

                var learner = _context.Learners.SingleOrDefault(u => u.Email == email);
                if (learner == null)
                {
                    return RedirectToAction("Login");
                }

                var directoryPath = Path.Combine("wwwroot", "images");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var filePath = Path.Combine(directoryPath, profilePicture.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await profilePicture.CopyToAsync(stream);
                }

                learner.ProfilePictureUrl = $"/images/{profilePicture.FileName}";
                _context.Update(learner);
                await _context.SaveChangesAsync();

                return RedirectToAction("Profile");
            }

            return RedirectToAction("Profile");
        }

       
    }
}
    


