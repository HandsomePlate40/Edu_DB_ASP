using Edu_DB_ASP.Models;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

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
        
        [HttpGet]
        public IActionResult Profile()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (email == null)
            {
                return RedirectToAction("LoginRoleSelection");
            }

            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole == "Learner")
            {
                return RedirectToAction("LearnerProfile");
            }
            else if (userRole == "Instructor")
            {
                return RedirectToAction("InstructorProfile");
            }
            else if (userRole == "Admin")
            {
                return RedirectToAction("AdminProfile");
            }

            return RedirectToAction("LoginRoleSelection");
        }
        
 


        [HttpGet]
        public IActionResult InstructorLogin()
        {
            return View();
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
                    if (_context.Learners.Any(a => a.Email == model.Email))
                    {
                        ModelState.AddModelError("Email", "A User with this email already exists.");
                        return View(model);
                    }

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
                    if (_context.Instructors.Any(a => a.Email == model.Email))
                    {
                        ModelState.AddModelError("Email", "An Instructor with this email already exists.");
                        return View(model);
                    }
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
        public IActionResult LearnerProfile()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (email == null)
            {
                return RedirectToAction("LearnerLogin");
            }

            var learner = _context.Learners.SingleOrDefault(u => u.Email == email);
            if (learner == null)
            {
                return RedirectToAction("LearnerLogin");
            }

            var enrolledCourses = _context.EnrolledCourseViewModels
                .FromSqlRaw("EXEC EnrolledCourses @LearnerID = {0}", learner.LearnerId)
                .ToList();

            var availableForums = _context.DiscussionForums.ToList(); // Fetch available forums

            var learningGoals = _context.LearningGoals
                .Where(lg => lg.LearnerId == learner.LearnerId)
                .ToList(); // Fetch learning goals

            var viewModel = new LearnerProfileViewModel
            {
                Learner = learner,
                EnrolledCourses = enrolledCourses,
                AvailableForums = availableForums,
                LearningGoals = learningGoals // Add learning goals to the view model
            };

            return View(viewModel);
        }

        
        [HttpPost]
        public async Task<IActionResult> UploadProfilePicture(IFormFile profilePicture)
        {
            try
            {
                if (profilePicture != null && profilePicture.Length > 0)
                {
                    var email = HttpContext.Session.GetString("UserEmail");
                    if (email == null)
                    {
                        return RedirectToAction("LearnerLogin");
                    }

                    var learner = _context.Learners.SingleOrDefault(u => u.Email == email);
                    if (learner == null)
                    {
                        return RedirectToAction("LearnerLogin");
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
                    _context.Learners.Update(learner);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("LearnerProfile");
                }

                ViewBag.ErrorMessage = "No profile picture was selected. Please choose a file and try again.";
                var noEmail = HttpContext.Session.GetString("UserEmail");
                if (noEmail != null)
                {
                    var noPicLearner = _context.Learners.SingleOrDefault(u => u.Email == noEmail);
                    var viewModel = new LearnerProfileViewModel
                    {
                        Learner = noPicLearner,
                        EnrolledCourses = new List<EnrolledCourseViewModel>()
                    };
                    return View("LearnerProfile", viewModel);
                }
                else
                {
                    return RedirectToAction("LearnerLogin");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occurred while uploading the profile picture: {ex.Message}";

                var email = HttpContext.Session.GetString("UserEmail");
                if (email != null)
                {
                    var learner = _context.Learners.SingleOrDefault(u => u.Email == email);
                    var viewModel = new LearnerProfileViewModel
                    {
                        Learner = learner,
                        EnrolledCourses = new List<EnrolledCourseViewModel>()
                    };
                    return View("LearnerProfile", viewModel);
                }
                else
                {
                    return RedirectToAction("LearnerLogin");
                }
            }

        }

        [HttpGet]
        public IActionResult LoginRoleSelection()
        {
            return View();
        }


        [HttpGet]
        public IActionResult RegisterAdmin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAdmin(RegisterViewModelAdmin model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (_context.Admins.Any(a => a.Email == model.Email))
                    {
                        ModelState.AddModelError("Email", "An admin with this email already exists.");
                        return View(model);
                    }

                    var hashedPassword = HashPassword(model.Password);
                    var admin = new Admin
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        PasswordHash = hashedPassword
                    };
                    _context.Admins.Add(admin);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("AdminLogin");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                }
            }
            
            return View(model);
        }


        [HttpGet]
        public IActionResult AdminLogin()
        {
            return View();
        }

        [HttpGet]
        public IActionResult InstructorProfile()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (email == null)
            {
                return RedirectToAction("InstructorLogin");
            }

            var instructor = _context.Instructors.SingleOrDefault(u => u.Email == email);
            if (instructor == null)
            {
                return RedirectToAction("InstructorLogin");
            }

            var taughtCourses = _context.Courses
                .FromSqlRaw("EXEC InstructorCourses @InstructorID = {0}", instructor.InstructorId)
                .ToList();

            var viewModel = new InstructorProfileViewModel
            {
                Instructor = instructor,
                TaughtCourses = taughtCourses
            };

            return View(viewModel);
        }

 [HttpPost]
public async Task<IActionResult> UploadInstructorProfilePicture(IFormFile profilePicture)
{
    try
    {
        if (profilePicture != null && profilePicture.Length > 0)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (email == null)
            {
                return RedirectToAction("InstructorLogin");
            }

            var instructor = _context.Instructors.SingleOrDefault(u => u.Email == email);
            if (instructor == null)
            {
                return RedirectToAction("InstructorLogin");
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

            instructor.ProfilePictureUrl = $"/images/{profilePicture.FileName}";
            _context.Instructors.Update(instructor);
            await _context.SaveChangesAsync();

            var taughtCourses = _context.Courses
                .FromSqlRaw("EXEC InstructorCourses @InstructorID = {0}", instructor.InstructorId)
                .ToList();

            var viewModel = new InstructorProfileViewModel
            {
                Instructor = instructor,
                TaughtCourses = taughtCourses
            };

            return View("InstructorProfile", viewModel);
        }

        ViewBag.ErrorMessage = "No profile picture was selected. Please choose a file and try again.";
        var noEmail = HttpContext.Session.GetString("UserEmail");
        if (noEmail != null)
        {
            var noPicInstructor = _context.Instructors.SingleOrDefault(u => u.Email == noEmail);
            var taughtCourses = _context.Courses
                .FromSqlRaw("EXEC InstructorCourses @InstructorID = {0}", noPicInstructor.InstructorId)
                .ToList();

            var viewModel = new InstructorProfileViewModel
            {
                Instructor = noPicInstructor,
                TaughtCourses = taughtCourses
            };

            return View("InstructorProfile", viewModel);
        }
        else
        {
            return RedirectToAction("InstructorLogin");
        }
    }
    catch (Exception ex)
    {
        ViewBag.ErrorMessage = $"An error occurred while uploading the profile picture: {ex.Message}";

        var email = HttpContext.Session.GetString("UserEmail");
        if (email != null)
        {
            var instructor = _context.Instructors.SingleOrDefault(u => u.Email == email);
            var taughtCourses = _context.Courses
                .FromSqlRaw("EXEC InstructorCourses @InstructorID = {0}", instructor.InstructorId)
                .ToList();

            var viewModel = new InstructorProfileViewModel
            {
                Instructor = instructor,
                TaughtCourses = taughtCourses
            };

            return View("InstructorProfile", viewModel);
        }
        else
        {
            return RedirectToAction("InstructorLogin");
        }
    }
}

        [HttpGet]
        public IActionResult AdminProfile()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (email == null)
            {
                return RedirectToAction("AdminLogin");
            }

            var admin = _context.Admins.SingleOrDefault(u => u.Email == email);
            if (admin == null)
            {
                return RedirectToAction("AdminLogin");
            }

            return View(admin);
        }

        [HttpPost]
        public async Task<IActionResult> UploadAdminProfilePicture(IFormFile profilePicture)
        {
            try
            {
                if (profilePicture != null && profilePicture.Length > 0)
                {
                    var email = HttpContext.Session.GetString("UserEmail");
                    if (email == null)
                    {
                        return RedirectToAction("AdminLogin");
                    }

                    var admin = _context.Admins.SingleOrDefault(u => u.Email == email);
                    if (admin == null)
                    {
                        return RedirectToAction("AdminLogin");
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

                    admin.ProfilePictureUrl = $"/images/{profilePicture.FileName}";
                    _context.Admins.Update(admin);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("AdminProfile");
                }

                ViewBag.ErrorMessage = "No profile picture was selected. Please choose a file and try again.";
                var noEmail = HttpContext.Session.GetString("UserEmail");
                if (noEmail != null)
                {
                    var noPicAdmin = _context.Admins.SingleOrDefault(u => u.Email == noEmail);
                    return View("AdminProfile", noPicAdmin);
                }
                else
                {
                    return RedirectToAction("AdminLogin");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occurred while uploading the profile picture: {ex.Message}";

                var email = HttpContext.Session.GetString("UserEmail");
                if (email != null)
                {
                    var admin = _context.Admins.SingleOrDefault(u => u.Email == email);
                    return View("AdminProfile", admin);
                }
                else
                {
                    return RedirectToAction("AdminLogin");
                }
            }
        }

        [HttpGet]
        public IActionResult LearnerIndex()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (email == null)
            {
                return RedirectToAction("LearnerLogin");
            }

            var learner = _context.Learners.SingleOrDefault(u => u.Email == email);
            if (learner == null)
            {
                return RedirectToAction("LearnerLogin");
            }

            return View(learner);
        }

        [HttpGet]
        public IActionResult PersonalizationProfileIndex()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (email == null)
            {
                return RedirectToAction("LearnerLogin");
            }

            var learner = _context.Learners.SingleOrDefault(u => u.Email == email);
            if (learner == null)
            {
                return RedirectToAction("LearnerLogin");
            }

            var personalizationProfiles = _context.PersonalizationProfiles
                .Where(p => p.LearnerId == learner.LearnerId)
                .ToList();
            return View(personalizationProfiles);
        }


        [HttpPost]
        public IActionResult LearnerLogin(LoginViewModel model)
        {
            var learner = _context.Learners.SingleOrDefault(u => u.Email == model.Email);

            if (learner != null && VerifyPassword(model.Password, learner.PasswordHash))
            {
                HttpContext.Session.SetString("UserEmail", learner.Email);
                HttpContext.Session.SetString("UserRole", "Learner");
                HttpContext.Session.SetInt32("LearnerId", learner.LearnerId); // Set LearnerId in session

                return RedirectToAction("LearnerProfile", "Account");
            }

            ModelState.AddModelError("", "Invalid email or password.");
            return View(model);
        }

        [HttpPost]
        public IActionResult InstructorLogin(LoginViewModel model)
        {
            var instructor = _context.Instructors.SingleOrDefault(u => u.Email == model.Email);

            if (instructor != null && VerifyPassword(model.Password, instructor.PasswordHash))
            {
                HttpContext.Session.SetString("UserEmail", instructor.Email);
                HttpContext.Session.SetString("UserRole", "Instructor");

                return RedirectToAction("InstructorProfile", "Account");
            }

            ModelState.AddModelError("", "Invalid email or password.");
            return View(model);
        }

        [HttpPost]
        public IActionResult AdminLogin(LoginViewModel model)
        {
            var admin = _context.Admins.SingleOrDefault(u => u.Email == model.Email);

            if (admin != null && VerifyPassword(model.Password, admin.PasswordHash))
            {
                HttpContext.Session.SetString("UserEmail", admin.Email);
                HttpContext.Session.SetString("UserRole", "Admin");

                return RedirectToAction("AdminProfile", "Account");
            }

            ModelState.AddModelError("", "Invalid email or password.");
            return View(model);
        }

    }
}
    


