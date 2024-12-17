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

namespace Edu_DB_ASP.Controllers.Course
{
    public class CourseController : Controller
    {
        private readonly EduDbContext _context;
        

        public CourseController(EduDbContext context)
        {
            _context = context;
            
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

        // GET: Course/Delete/5
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync("EXEC CourseRemove @courseID = {0}", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework or simply output to console)
                Console.WriteLine(ex.Message);
                // Optionally, return an error view or message
                return View("Error",
                    new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
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
    }
}

