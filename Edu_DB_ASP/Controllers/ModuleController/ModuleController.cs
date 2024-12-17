using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Edu_DB_ASP.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Edu_DB_ASP.Controllers.ModuleController
{
    public class ModuleController : Controller
    {
        private readonly EduDbContext _context;
        private readonly IConfiguration _configuration;
        public ModuleController(EduDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: Module
        public async Task<IActionResult> Index()
        {
            var eduDbContext = _context.Modules.Include(m => m.Course);
            return View(await eduDbContext.ToListAsync());
        }

        // GET: Module/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var module = await _context.Modules
                .Include(m => m.Course)
                .FirstOrDefaultAsync(m => m.ModuleId == id);
            if (module == null)
            {
                return NotFound();
            }

            return View(module);
        }

        // GET: Module/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId");
            return View();
        }

        // POST: Module/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ModuleId,Title,ContentType,ContentUrl,ModuleDifficulty,CourseId")] Module module)
        {
           // if (ModelState.IsValid)
           // {
                try
                {
                    _context.Add(module);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"An error occurred while creating the module: {ex.Message}");
                }
          //  }
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId", module.CourseId);
            return View(module);
        }

        // GET: Module/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var module = await _context.Modules.FindAsync(id);
            if (module == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId", module.CourseId);
            return View(module);
        }

        // POST: Module/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ModuleId,Title,ContentType,ContentUrl,ModuleDifficulty,CourseId")] Module module)
        {
            if (id != module.ModuleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(module);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModuleExists(module.ModuleId))
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
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId", module.CourseId);
            return View(module);
        }

        // GET: Module/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var module = await _context.Modules
                .Include(m => m.Course)
                .FirstOrDefaultAsync(m => m.ModuleId == id);
            if (module == null)
            {
                return NotFound();
            }

            return View(module);
        }

        // POST: Module/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var module = await _context.Modules.FindAsync(id);
            if (module != null)
            {
                _context.Modules.Remove(module);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModuleExists(int id)
        {
            return _context.Modules.Any(e => e.ModuleId == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddActivity(int moduleId, string activityType, string instructionDetails, int maxPoints)
        {
            var courseId = _context.Modules.Where(m => m.ModuleId == moduleId).Select(m => m.CourseId).FirstOrDefault();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (var command = new SqlCommand("NewActivity", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CourseID", courseId);
                    command.Parameters.AddWithValue("@ModuleID", moduleId);
                    command.Parameters.AddWithValue("@activitytype", activityType);
                    command.Parameters.AddWithValue("@instructiondetails", instructionDetails);
                    command.Parameters.AddWithValue("@maxpoints", maxPoints);

                    connection.Open();
                    await command.ExecuteNonQueryAsync();
                }
            }

            return RedirectToAction(nameof(Edit), new { id = moduleId });
        }

        public async Task<IActionResult> ViewModules(int courseId)
        {
            var modules = await _context.Modules
                .Where(m => m.CourseId == courseId)
                .ToListAsync();

            return View(modules);
        }
    }
}
