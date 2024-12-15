using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Edu_DB_ASP.Models;
using Microsoft.Data.SqlClient;
namespace Edu_DB_ASP.Controllers.LearnerOptions
{
    public class LearnersController : Controller
    {
        private readonly EduDbContext _context;

        public LearnersController(EduDbContext context)
        {
            _context = context;
        }

        // GET: Learners
        public async Task<IActionResult> Index()
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

            return View(learner);
        }


        // GET: Learners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learner = await _context.Learners
                .FirstOrDefaultAsync(m => m.LearnerId == id);
            if (learner == null)
            {
                return NotFound();
            }

            return View(learner);
        }

        // GET: Learners/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Learners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LearnerId,FirstName,LastName,Gender,CountryOfOrigin,CulturalBackground,PersonalityTraits,EmotionalProfile,PhysicalHealth,MentalHealth,ExperienceLevel,Email,PasswordHash,ProfilePictureUrl")] Learner learner)
        {
            if (ModelState.IsValid)
            {
                _context.Add(learner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(learner);
        }

        // GET: Learners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learner = await _context.Learners.FindAsync(id);
            if (learner == null)
            {
                return NotFound();
            }
            return View(learner);
        }

        // POST: Learners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LearnerId,FirstName,LastName,Gender,CountryOfOrigin,CulturalBackground,PersonalityTraits,EmotionalProfile,PhysicalHealth,MentalHealth,ExperienceLevel,Email,PasswordHash,ProfilePictureUrl")] Learner learner)
        {
            if (id != learner.LearnerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(learner);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LearnerExists(learner.LearnerId))
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
            return View(learner);
        }

        // GET: Learners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learner = await _context.Learners
                .FirstOrDefaultAsync(m => m.LearnerId == id);
            if (learner == null)
            {
                return NotFound();
            }

            return View(learner);
        }

        // POST: Learners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var learner = await _context.Learners.FindAsync(id);
            if (learner != null)
            {
                _context.Learners.Remove(learner);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LearnerExists(int id)
        {
            return _context.Learners.Any(e => e.LearnerId == id);
        }
        
        public async Task<IActionResult> JoinQuest(int questId)
        {
            var learnerEmail = HttpContext.Session.GetString("UserEmail");
            if (learnerEmail == null)
            {
                return RedirectToAction("LearnerLogin", "Account");
            }

            var learner = await _context.Learners.SingleOrDefaultAsync(u => u.Email == learnerEmail);
            if (learner == null)
            {
                return RedirectToAction("LearnerLogin", "Account");
            }

            var learnerId = learner.LearnerId;

            var sql = "EXEC JoinQuest @LearnerID, @QuestID";
            var parameters = new[]
            {
                new SqlParameter("@LearnerID", learnerId),
                new SqlParameter("@QuestID", questId)
            };

            await _context.Database.ExecuteSqlRawAsync(sql, parameters);

            return RedirectToAction("JoinCollabQuest");
        }
    
        public async Task<IActionResult> AvailableQuests()
        {
            var quests = await _context.CollaborativeQuests
                .Select(q => new CollaborativeQuest
                {
                    QuestId = q.QuestId,
                    MaxParticipants = q.MaxParticipants,
                    DeadLine = q.DeadLine,
                    Quest = q.Quest,
                })
                .ToListAsync();

            return View(quests);
        }
        
        public async Task<IActionResult> JoinCollabQuest()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Learner")
            {
                return RedirectToAction("LearnerLogin", "Account");
            }

            var quests = await _context.Quests
                .Where(q => q.QuestType == "Collaborative_Quest")
                .Select(q => new CollaborativeQuest
                {
                    QuestId = q.QuestId,
                    MaxParticipants = q.CollaborativeQuest.MaxParticipants,
                    DeadLine = q.CollaborativeQuest.DeadLine,
                    Quest = q
                })
                .ToListAsync();

            return View(quests);
        }
    }
}
