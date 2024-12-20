﻿using Edu_DB_ASP.Models;
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
        public async Task<IActionResult> LearnerProfile()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (email == null)
            {
                return RedirectToAction("LearnerLogin", "Account");
            }

            var learner = await _context.Learners.SingleOrDefaultAsync(u => u.Email == email);
            if (learner == null)
            {
                return RedirectToAction("LearnerLogin", "Account");
            }

            var learnerId = learner.LearnerId;

            var learningPath = await _context.LearningPaths
                .FromSqlRaw("EXEC CurrentPath @LearnerID = {0}", learnerId)
                .ToListAsync();

            var enrolledCourses = await _context.CourseEnrollments
                .Where(ec => ec.LearnerId == learnerId)
                .Select(ec => new EnrolledCourseViewModel
                {
                    CourseId = ec.CourseId,
                    Title = ec.Course.Title,
                    CourseDescription = ec.Course.CourseDescription,
                    DifficultyLevel = ec.Course.DifficultyLevel,
                    CreditPoints = ec.Course.CreditPoints.HasValue ? (int)ec.Course.CreditPoints.Value : 0,
                })
                .ToListAsync();

            var availableForums = await _context.DiscussionForums.ToListAsync();

            var learningGoals = await _context.LearningGoals
                .Where(lg => lg.LearnerId == learnerId)
                .ToListAsync();

            var viewModel = new LearnerProfileViewModel
            {
                Learner = learner,
                LearningPath = learningPath ?? new List<LearningPath>(),
                EnrolledCourses = enrolledCourses ?? new List<EnrolledCourseViewModel>(),
                AvailableForums = availableForums ?? new List<DiscussionForum>(),
                LearningGoals = learningGoals ?? new List<LearningGoal>()
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
        public async Task<IActionResult> InstructorProfile()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (email == null)
            {
                return RedirectToAction("InstructorLogin");
            }

            var instructor = await _context.Instructors.SingleOrDefaultAsync(u => u.Email == email);
            if (instructor == null)
            {
                return RedirectToAction("InstructorLogin");
            }

            var taughtCourses = await _context.Courses
                .FromSqlRaw("EXEC InstructorCourses @InstructorID = {0}", instructor.InstructorId)
                .ToListAsync();

            var availableForums = await _context.DiscussionForums.ToListAsync();

            var viewModel = new InstructorProfileViewModel
            {
                Instructor = instructor,
                TaughtCourses = taughtCourses,
                AvailableForums = availableForums ?? new List<DiscussionForum>(),
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

            var taughtCourses = await _context.Courses
                .FromSqlRaw("EXEC InstructorCourses @InstructorID = {0}", instructor.InstructorId)
                .ToListAsync();

            var availableForums = await _context.DiscussionForums.ToListAsync();

            var viewModel = new InstructorProfileViewModel
            {
                Instructor = instructor,
                TaughtCourses = taughtCourses,
                AvailableForums = availableForums ?? new List<DiscussionForum>(),
            };

            return View("InstructorProfile", viewModel);
        }

        ViewBag.ErrorMessage = "No profile picture was selected. Please choose a file and try again.";
        return RedirectToAction("InstructorProfile");
    }
    catch (Exception ex)
    {
        ViewBag.ErrorMessage = $"An error occurred while uploading the profile picture: {ex.Message}";
        return RedirectToAction("InstructorProfile");
    }
}

[HttpGet]
public async Task<IActionResult> AdminProfile()
{
    var email = HttpContext.Session.GetString("UserEmail");
    if (email == null)
    {
        return RedirectToAction("AdminLogin");
    }

    var admin = await _context.Admins.SingleOrDefaultAsync(u => u.Email == email);
    if (admin == null)
    {
        return RedirectToAction("AdminLogin");
    }

    var firstFiveLearners = await _context.Learners
        .OrderBy(l => l.LearnerId)
        .Take(5)
        .ToListAsync();

    var firstFiveInstructors = await _context.Instructors
        .OrderBy(i => i.InstructorId)
        .Take(5)
        .ToListAsync();

    var viewModel = new AdminProfileViewModel
    {
        Admin = admin,
        FirstFiveLearners = firstFiveLearners,
        FirstFiveInstructors = firstFiveInstructors
    };

    return View(viewModel);
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

            var firstFiveLearners = await _context.Learners
                .OrderBy(l => l.LearnerId)
                .Take(5)
                .ToListAsync();

            var firstFiveInstructors = await _context.Instructors
                .OrderBy(i => i.InstructorId)
                .Take(5)
                .ToListAsync();

            var viewModel = new AdminProfileViewModel
            {
                Admin = admin,
                FirstFiveLearners = firstFiveLearners,
                FirstFiveInstructors = firstFiveInstructors
            };

            return View("AdminProfile", viewModel);
        }

        ViewBag.ErrorMessage = "No profile picture was selected. Please choose a file and try again.";
        var noEmail = HttpContext.Session.GetString("UserEmail");
        if (noEmail != null)
        {
            var noPicAdmin = _context.Admins.SingleOrDefault(u => u.Email == noEmail);
            var firstFiveLearners = await _context.Learners
                .OrderBy(l => l.LearnerId)
                .Take(5)
                .ToListAsync();

            var firstFiveInstructors = await _context.Instructors
                .OrderBy(i => i.InstructorId)
                .Take(5)
                .ToListAsync();

            var viewModel = new AdminProfileViewModel
            {
                Admin = noPicAdmin,
                FirstFiveLearners = firstFiveLearners,
                FirstFiveInstructors = firstFiveInstructors
            };

            return View("AdminProfile", viewModel);
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
            var firstFiveLearners = await _context.Learners
                .OrderBy(l => l.LearnerId)
                .Take(5)
                .ToListAsync();

            var firstFiveInstructors = await _context.Instructors
                .OrderBy(i => i.InstructorId)
                .Take(5)
                .ToListAsync();

            var viewModel = new AdminProfileViewModel
            {
                Admin = admin,
                FirstFiveLearners = firstFiveLearners,
                FirstFiveInstructors = firstFiveInstructors
            };

            return View("AdminProfile", viewModel);
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
                HttpContext.Session.SetInt32("InstructorId", instructor.InstructorId); // Set InstructorId in session

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
                HttpContext.Session.SetInt32("AdminId", admin.AdminId);

                return RedirectToAction("AdminProfile", "Account");
            }

            ModelState.AddModelError("", "Invalid email or password.");
            return View(model);
        }
    }
}
    


