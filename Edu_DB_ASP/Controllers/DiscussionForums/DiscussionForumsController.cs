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
        public async Task<IActionResult> Create(
            [Bind("ForumId,Title,Description,LastActiveTimestamp,ModuleId")]
            DiscussionForum discussionForum)
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
        public async Task<IActionResult> Edit(int id,
            [Bind("ForumId,Title,Description,LastActiveTimestamp,ModuleId")]
            DiscussionForum discussionForum)
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
            var learnerMessages = await _context.Joins
                .Where(j => j.ForumId == forumId)
                .Select(j => new MessageViewModel
                {
                    UserName = j.Learner.FirstName + " " + j.Learner.LastName,
                    Content = j.Post,
                    ProfilePictureUrl = j.Learner.ProfilePictureUrl,
                    UserRole = "Learner"
                })
                .ToListAsync();

            var instructorMessages = await _context.InstructorJoins
                .Where(ij => ij.ForumId == forumId)
                .Select(ij => new MessageViewModel
                {
                    UserName = ij.Instructor.InstructorName,
                    Content = ij.Post,
                    ProfilePictureUrl = ij.Instructor.ProfilePictureUrl,
                    UserRole = "Instructor"
                })
                .ToListAsync();

            var viewModel = new PostMessageViewModel
            {
                ForumId = forumId,
                Messages = learnerMessages.Concat(instructorMessages).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> PostMessage(PostMessageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userRole = HttpContext.Session.GetString("UserRole");
                if (userRole == "Learner")
                {
                    var learnerId = HttpContext.Session.GetInt32("LearnerId");
                    if (learnerId == null)
                    {
                        return RedirectToAction("LearnerLogin", "Account");
                    }

                    var existingPost = await _context.Joins
                        .FirstOrDefaultAsync(j => j.ForumId == model.ForumId && j.LearnerId == learnerId);

                    if (existingPost != null)
                    {
                        ModelState.AddModelError("", "You have already posted in this forum.");
                        await ReloadMessages(model);
                        return View(model);
                    }

                    var result = await _context.Database.ExecuteSqlRawAsync(
                        "EXEC Post @LearnerID = {0}, @DiscussionID = {1}, @Post = {2}",
                        learnerId, model.ForumId, model.Post);

              
                }
                else if (userRole == "Instructor")
                {
                    var instructorId = HttpContext.Session.GetInt32("InstructorId");
                    if (instructorId == null)
                    {
                        return RedirectToAction("InstructorLogin", "Account");
                    }

                    var existingPost = await _context.InstructorJoins
                        .FirstOrDefaultAsync(j => j.ForumId == model.ForumId && j.InstructorId == instructorId);

                    if (existingPost != null)
                    {
                        ModelState.AddModelError("", "You have already posted in this forum.");
                        await ReloadMessages(model);
                        return View(model);
                    }

                    var result = await _context.Database.ExecuteSqlRawAsync(
                        "EXEC PostInstructor @InstructorID = {0}, @DiscussionID = {1}, @Post = {2}",
                        instructorId, model.ForumId, model.Post);
                    
                }

                return RedirectToAction("PostMessage", new { forumId = model.ForumId });
            }

            await ReloadMessages(model);
            return View(model);
        }

        private async Task ReloadMessages(PostMessageViewModel model)
        {
            var learnerMessages = await _context.Joins
                .Where(j => j.ForumId == model.ForumId)
                .Select(j => new MessageViewModel
                {
                    UserName = j.Learner.FirstName + " " + j.Learner.LastName,
                    Content = j.Post,
                    ProfilePictureUrl = j.Learner.ProfilePictureUrl,
                    UserRole = "Learner"
                })
                .ToListAsync();

            var instructorMessages = await _context.InstructorJoins
                .Where(ij => ij.ForumId == model.ForumId)
                .Select(ij => new MessageViewModel
                {
                    UserName = ij.Instructor.InstructorName,
                    Content = ij.Post,
                    ProfilePictureUrl = ij.Instructor.ProfilePictureUrl,
                    UserRole = "Instructor"
                })
                .ToListAsync();

            model.Messages = learnerMessages.Concat(instructorMessages).ToList();

        }

        [HttpGet]
        public async Task<IActionResult> EditMessage(int userId, string userRole)
        {
            if (userRole == "Learner")
            {
                var post = await _context.Joins
                    .Where(j => j.LearnerId == userId)
                    .Select(j => new MessageViewModel
                    {
                        UserId = j.LearnerId,
                        UserName = j.Learner.FirstName + " " + j.Learner.LastName,
                        Content = j.Post,
                        ProfilePictureUrl = j.Learner.ProfilePictureUrl,
                        UserRole = "Learner"
                    })
                    .FirstOrDefaultAsync();

                if (post == null)
                {
                    return NotFound();
                }

                return View(post);
            }
            else if (userRole == "Instructor")
            {
                var post = await _context.InstructorJoins
                    .Where(ij => ij.InstructorId == userId)
                    .Select(ij => new MessageViewModel
                    {
                        UserId = ij.InstructorId,
                        UserName = ij.Instructor.InstructorName,
                        Content = ij.Post,
                        ProfilePictureUrl = ij.Instructor.ProfilePictureUrl,
                        UserRole = "Instructor"
                    })
                    .FirstOrDefaultAsync();

                if (post == null)
                {
                    return NotFound();
                }

                return View(post);
            }

            return BadRequest();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMessage(int userId, string userRole, string content)
        {
            if (userRole == "Learner")
            {
                var post = await _context.Joins.FirstOrDefaultAsync(j => j.LearnerId == userId);
                if (post == null)
                {
                    return NotFound();
                }

                post.Post = content;
                await _context.SaveChangesAsync();
                return RedirectToAction("PostMessage", new { forumId = post.ForumId });
            }
            else if (userRole == "Instructor")
            {
                var post = await _context.InstructorJoins.FirstOrDefaultAsync(ij => ij.InstructorId == userId);
                if (post == null)
                {
                    return NotFound();
                }

                post.Post = content;
                await _context.SaveChangesAsync();
                return RedirectToAction("PostMessage", new { forumId = post.ForumId });
            }

            return BadRequest();
        }


        public IActionResult DeleteDiscussion(int id)
        {
            return RedirectToAction("Delete", new { id });
        }
    }
}
    
