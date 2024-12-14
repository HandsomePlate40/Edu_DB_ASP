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
            ViewData["NotificationId"] = new SelectList(_context.Notifications, "NotificationId", "NotificationId");
            ViewData["PathId"] = new SelectList(_context.LearningPaths, "PathId", "PathId");
            return View();
        }

        // POST: PersonalizationProfiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CreationOrder,LearnerId,PersonalityType,EmotionalState,AccessibilityPreferences,PreferredContentTypes,NotificationId,PathId")] PersonalizationProfile personalizationProfile)
        {
            if (ModelState.IsValid)
            {
                _context.Add(personalizationProfile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LearnerId"] = new SelectList(_context.Learners, "LearnerId", "LearnerId", personalizationProfile.LearnerId);
            ViewData["NotificationId"] = new SelectList(_context.Notifications, "NotificationId", "NotificationId", personalizationProfile.NotificationId);
            ViewData["PathId"] = new SelectList(_context.LearningPaths, "PathId", "PathId", personalizationProfile.PathId);
            return View(personalizationProfile);
        }

        // GET: PersonalizationProfiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personalizationProfile = await _context.PersonalizationProfiles.FindAsync(id);
            if (personalizationProfile == null)
            {
                return NotFound();
            }
            ViewData["LearnerId"] = new SelectList(_context.Learners, "LearnerId", "LearnerId", personalizationProfile.LearnerId);
            ViewData["NotificationId"] = new SelectList(_context.Notifications, "NotificationId", "NotificationId", personalizationProfile.NotificationId);
            ViewData["PathId"] = new SelectList(_context.LearningPaths, "PathId", "PathId", personalizationProfile.PathId);
            return View(personalizationProfile);
        }

        // POST: PersonalizationProfiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CreationOrder,LearnerId,PersonalityType,EmotionalState,AccessibilityPreferences,PreferredContentTypes,NotificationId,PathId")] PersonalizationProfile personalizationProfile)
        {
            if (id != personalizationProfile.CreationOrder)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(personalizationProfile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonalizationProfileExists(personalizationProfile.CreationOrder))
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
            ViewData["LearnerId"] = new SelectList(_context.Learners, "LearnerId", "LearnerId", personalizationProfile.LearnerId);
            ViewData["NotificationId"] = new SelectList(_context.Notifications, "NotificationId", "NotificationId", personalizationProfile.NotificationId);
            ViewData["PathId"] = new SelectList(_context.LearningPaths, "PathId", "PathId", personalizationProfile.PathId);
            return View(personalizationProfile);
        }

        // GET: PersonalizationProfiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: PersonalizationProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var personalizationProfile = await _context.PersonalizationProfiles.FindAsync(id);
            if (personalizationProfile != null)
            {
                _context.PersonalizationProfiles.Remove(personalizationProfile);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonalizationProfileExists(int id)
        {
            return _context.PersonalizationProfiles.Any(e => e.CreationOrder == id);
        }
    }
}
