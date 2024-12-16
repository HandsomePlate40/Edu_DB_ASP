using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Edu_DB_ASP.Models;

namespace Edu_DB_ASP.Controllers.DiscussionForums
{
    public class DiscussionForumsController : Controller
    {
        private readonly EduDbContext _context;

        public DiscussionForumsController(EduDbContext context)
        {
            _context = context;
        }

        // GET: DiscussionForums
        public async Task<IActionResult> Index()
        {
            var eduDbContext = _context.DiscussionForums.Include(d => d.Module);
            return View(await eduDbContext.ToListAsync());
        }

        // GET: DiscussionForums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discussionForum = await _context.DiscussionForums
                .Include(d => d.Module)
                .FirstOrDefaultAsync(m => m.ForumId == id);
            if (discussionForum == null)
            {
                return NotFound();
            }

            return View(discussionForum);
        }

        // GET: DiscussionForums/Create
        public IActionResult Create()
        {
            ViewData["ModuleId"] = new SelectList(_context.Modules, "ModuleId", "ModuleId");
            return View();
        }

        // POST: DiscussionForums/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ForumId,Title,Description,LastActiveTimestamp,ModuleId")] DiscussionForum discussionForum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(discussionForum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ModuleId"] = new SelectList(_context.Modules, "ModuleId", "ModuleId", discussionForum.ModuleId);
            return View(discussionForum);
        }

        // GET: DiscussionForums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discussionForum = await _context.DiscussionForums.FindAsync(id);
            if (discussionForum == null)
            {
                return NotFound();
            }
            ViewData["ModuleId"] = new SelectList(_context.Modules, "ModuleId", "ModuleId", discussionForum.ModuleId);
            return View(discussionForum);
        }

        // POST: DiscussionForums/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ForumId,Title,Description,LastActiveTimestamp,ModuleId")] DiscussionForum discussionForum)
        {
            if (id != discussionForum.ForumId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(discussionForum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiscussionForumExists(discussionForum.ForumId))
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
            ViewData["ModuleId"] = new SelectList(_context.Modules, "ModuleId", "ModuleId", discussionForum.ModuleId);
            return View(discussionForum);
        }

        // GET: DiscussionForums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discussionForum = await _context.DiscussionForums
                .Include(d => d.Module)
                .FirstOrDefaultAsync(m => m.ForumId == id);
            if (discussionForum == null)
            {
                return NotFound();
            }

            return View(discussionForum);
        }

        // POST: DiscussionForums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var discussionForum = await _context.DiscussionForums.FindAsync(id);
            if (discussionForum != null)
            {
                _context.DiscussionForums.Remove(discussionForum);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiscussionForumExists(int id)
        {
            return _context.DiscussionForums.Any(e => e.ForumId == id);
        }

        [HttpGet]
        public async Task<IActionResult> PostMessage(int forumId)
        {
            var forum = await _context.DiscussionForums
                .FirstOrDefaultAsync(f => f.ForumId == forumId);

            if (forum == null)
            {
                return NotFound();
            }

            var messages = await _context.Joins
                .Where(j => j.ForumId == forumId)
                .Select(j => new MessageViewModel
                {
                    LearnerName = j.Learner.FirstName + " " + j.Learner.LastName,
                    Content = j.Post,
                    ProfilePictureUrl = j.Learner.ProfilePictureUrl // Assuming this property exists
                })
                .ToListAsync();

            var viewModel = new PostMessageViewModel
            {
                ForumId = forumId,
                Messages = messages
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> PostMessage(PostMessageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var learnerId = HttpContext.Session.GetInt32("LearnerId");
                if (learnerId == null)
                {
                    return RedirectToAction("LearnerLogin", "Account");
                }

                // Check if the learner has already posted in this forum
                var existingPost = await _context.Joins
                    .FirstOrDefaultAsync(j => j.ForumId == model.ForumId && j.LearnerId == learnerId);

                if (existingPost != null)
                {
                    ModelState.AddModelError("", "You have already posted in this forum.");
                    return View(model);
                }

                var result = await _context.Database.ExecuteSqlRawAsync(
                    "EXEC Post @LearnerID = {0}, @DiscussionID = {1}, @Post = {2}",
                    learnerId, model.ForumId, model.Post);

                if (result == -1)
                {
                    ModelState.AddModelError("", "An error occurred while posting the message.");
                    return View(model);
                }

                return RedirectToAction("PostMessage", new { forumId = model.ForumId });
            }

            return View(model);
        }
    }
}