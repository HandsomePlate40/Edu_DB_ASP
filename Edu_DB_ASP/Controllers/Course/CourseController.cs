using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Edu_DB_ASP.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Claims;

namespace Edu_DB_ASP.Controllers.Course
{
    public class CourseController : Controller
    {
        private readonly EduDbContext _context;
        private readonly IConfiguration _configuration;

        public CourseController(EduDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: Course
        public async Task<IActionResult> Index()
        {
            return View(await _context.Courses.ToListAsync());
        }

        // GET: Course/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Course/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Course/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("CourseId,Title,CourseDescription,DifficultyLevel,Prerequisites,CreditPoints,LearningObjectives")]
            Models.Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(course);
        }

        // GET: Course/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Course/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("CourseId,Title,CourseDescription,DifficultyLevel,Prerequisites,CreditPoints,LearningObjectives")]
            Models.Course course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(course);
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enroll(int courseId)
        {
            var learnerId = HttpContext.Session.GetInt32("LearnerId");
            if (learnerId == null)
            {
                return RedirectToAction("LearnerLogin", "Account");
            }

            var isEnrolled = await _context.CourseEnrollments
                .AnyAsync(e => e.LearnerId == learnerId && e.CourseId == courseId);

            if (isEnrolled)
            {
                TempData["Error"] = "Already enrolled in this course.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC EnrollLearnerToCourse @LearnerID = {0}, @CourseID = {1}", learnerId, courseId);
                TempData["Message"] = "Successfully enrolled in the course.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error enrolling in the course: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
        
        [HttpGet]
        public async Task<IActionResult> CheckPrerequisites(int learnerId, int courseId)
        {
            string message;
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("Prerequisites", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@LearnerID", learnerId);
                    command.Parameters.AddWithValue("@CourseID", courseId);

                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            message = reader["Message"].ToString();
                        }
                        else
                        {
                            message = "Error: Unable to check prerequisites.";
                        }
                    }
                }
            }

            return Ok(new { Message = message });
        }

        public async Task<IActionResult> ViewPreviousCourses()
        {
            var learnerId = HttpContext.Session.GetInt32("LearnerId");
            if (learnerId == null)
            {
                return RedirectToAction("LearnerLogin", "Account");
            }

            var previousCourses = new List<PreviousCourseViewModel>();
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("ViewPreviousCourseDetailsLearner", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@LearnerID", learnerId);
                    command.Parameters.AddWithValue("@CourseID", DBNull.Value); // Assuming you want all courses

                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            previousCourses.Add(new PreviousCourseViewModel
                            {
                                CourseId = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                CourseDescription = reader.GetString(2),
                                DifficultyLevel = reader.GetString(3),
                                CreditPoints = reader.GetDecimal(4),
                                EnrollmentDate = reader.GetDateTime(5),
                                CompletionDate = reader.GetDateTime(6)
                            });
                        }
                    }
                }
            }

            return View(previousCourses);
        }
        [HttpGet]
        public IActionResult Unenroll(int courseId)
        {
            var learnerId = HttpContext.Session.GetInt32("LearnerId");
            if (learnerId == null)
            {
                TempData["Error"] = "Learner not logged in.";
                return RedirectToAction("LearnerLogin", "Account");
            }

            return View(courseId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmUnenroll(int courseId)
        {
            var learnerId = HttpContext.Session.GetInt32("LearnerId");
            if (learnerId == null)
            {
                TempData["Error"] = "Learner not logged in.";
                return RedirectToAction("LearnerLogin", "Account");
            }

            try
            {
                var enrollment = await _context.CourseEnrollments
                    .FirstOrDefaultAsync(e => e.LearnerId == learnerId && e.CourseId == courseId);

                if (enrollment == null)
                {
                    TempData["Error"] = "Enrollment not found.";
                    return RedirectToAction(nameof(Index));
                }

                _context.CourseEnrollments.Remove(enrollment);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Successfully unenrolled from the course.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error unenrolling from the course: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
        
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCourse(int courseId)
        {
            string message;
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand cmd = new SqlCommand("CourseRemove", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@courseID", courseId);
                    SqlParameter outputMessage = new SqlParameter("@Message", SqlDbType.NVarChar, 100)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputMessage);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    message = outputMessage.Value.ToString();
                }
            }

            TempData["Message"] = message;
            return RedirectToAction("InstructorProfile", "Account");
        }
    }
}


