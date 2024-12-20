using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Edu_DB_ASP.Models;

namespace Edu_DB_ASP.Controllers.PersonalizationProfiles
{
    public class PersonalizationProfilesController : Controller
    {
        private readonly EduDbContext _context;

        public PersonalizationProfilesController(EduDbContext context)
        {
            _context = context;
        }

        // GET: PersonalizationProfiles
        public async Task<IActionResult> Index()
        {
            var eduDbContext = _context.PersonalizationProfiles.Include(p => p.Learner).Include(p => p.Notification).Include(p => p.Path);
            return View(await eduDbContext.ToListAsync());
        }

        // GET: PersonalizationProfiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personalizationProfile = await _context.PersonalizationProfiles
                .Include(p => p.Learner)
                .Include(p => p.Notification)
                .Include(p => p.Path)
                .FirstOrDefaultAsync(m => m.CreationOrder == id);
            if (personalizationProfile == null)
            {
                return NotFound();
            }

            return View(personalizationProfile);
        }

        // GET: PersonalizationProfiles/Create
        public IActionResult Create()
        {
            ViewData["LearnerId"] = new SelectList(_context.Learners, "LearnerId", "LearnerId");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CreationOrder,PersonalityType,EmotionalState,AccessibilityPreferences,PreferredContentTypes")] PersonalizationProfile personalizationProfile)
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

            personalizationProfile.LearnerId = learner.LearnerId;

            ModelState.Remove("Learner"); // Remove the Learner property from validation

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(personalizationProfile);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the profile: " + ex.Message);
                }
            }
            else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    ModelState.AddModelError(string.Empty, error.ErrorMessage);
                }
            }

            ViewData["LearnerId"] = new SelectList(_context.Learners, "LearnerId", "LearnerId", personalizationProfile.LearnerId);
            return View(personalizationProfile);
        }
        
        // GET: PersonalizationProfiles/Edit/5
public async Task<IActionResult> Edit(int? creationOrder, int? learnerId)
{
    if (creationOrder == null || learnerId == null)
    {
        return NotFound();
    }

    var personalizationProfile = await _context.PersonalizationProfiles
        .Include(p => p.Learner)
        .FirstOrDefaultAsync(m => m.CreationOrder == creationOrder && m.LearnerId == learnerId);
    if (personalizationProfile == null)
    {
        return NotFound();
    }

    return View(personalizationProfile);
}

// POST: PersonalizationProfiles/Edit/5
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int creationOrder, int learnerId, [Bind("CreationOrder,PersonalityType,EmotionalState,AccessibilityPreferences,PreferredContentTypes")] PersonalizationProfile personalizationProfile)
{
    if (creationOrder != personalizationProfile.CreationOrder || learnerId != personalizationProfile.LearnerId)
    {
        return BadRequest();
    }

    if (ModelState.IsValid)
    {
        try
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

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC ProfileUpdate @LearnerID = {0}, @ProfileID = {1}, @PreferredContentType = {2}, @EmotionalState = {3}, @PersonalityType = {4}",
                learnerId, creationOrder, personalizationProfile.PreferredContentTypes, personalizationProfile.EmotionalState, personalizationProfile.PersonalityType);

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "An error occurred while updating the profile: " + ex.Message);
        }
    }
    else
    {
        foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
        {
            ModelState.AddModelError(string.Empty, error.ErrorMessage);
        }
    }

    return View(personalizationProfile);
}
        
        private bool PersonalizationProfileExists(int id)
        {
            return _context.PersonalizationProfiles.Any(e => e.CreationOrder == id);
        }

        // GET: PersonalizationProfiles/Delete/5/1
        public async Task<IActionResult> Delete(int? creationOrder, int? learnerId)
        {
            if (creationOrder == null || learnerId == null)
            {
                return NotFound();
            }

            var personalizationProfile = await _context.PersonalizationProfiles
                .Include(p => p.Learner)
                .Include(p => p.Notification)
                .Include(p => p.Path)
                .FirstOrDefaultAsync(m => m.CreationOrder == creationOrder && m.LearnerId == learnerId);
            if (personalizationProfile == null)
            {
                return NotFound();
            }

            return View(personalizationProfile);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int creationOrder, int learnerId)
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

            var personalizationProfile = await _context.PersonalizationProfiles.FindAsync(creationOrder, learnerId);
            if (personalizationProfile != null)
            {
                _context.PersonalizationProfiles.Remove(personalizationProfile);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
