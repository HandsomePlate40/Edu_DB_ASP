using System;
using System.Collections.Generic;
using System.Data;
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
        private readonly QuestRepository _questRepository;
        private readonly string _connectionString;

        public LearnersController(EduDbContext context, QuestRepository questRepository, IConfiguration configuration)
        {
            _context = context;
            _questRepository = questRepository;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "LearnerId,FirstName,LastName,Gender,CountryOfOrigin,CulturalBackground,PersonalityTraits,EmotionalProfile,PhysicalHealth,MentalHealth,ExperienceLevel,Email,PasswordHash,ProfilePictureUrl")]
            Learner learner)
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind(
                "LearnerId,FirstName,LastName,Gender,CountryOfOrigin,CulturalBackground,PersonalityTraits,EmotionalProfile,PhysicalHealth,MentalHealth,ExperienceLevel,Email,PasswordHash,ProfilePictureUrl")]
            Learner learner)
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

            var messageParam = new SqlParameter
            {
                ParameterName = "@Message",
                SqlDbType = System.Data.SqlDbType.NVarChar,
                Size = 100,
                Direction = System.Data.ParameterDirection.Output
            };

            var sql = "EXEC JoinQuest @LearnerID, @QuestID, @Message OUTPUT";
            var parameters = new[]
            {
                new SqlParameter("@LearnerID", learnerId),
                new SqlParameter("@QuestID", questId),
                messageParam
            };

            await _context.Database.ExecuteSqlRawAsync(sql, parameters);

            TempData["ErrorMessage"] = messageParam.Value.ToString();
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

        public async Task<IActionResult> Notifications()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (email == null)
            {
                return RedirectToAction("LearnerLogin", "Account");
            }

            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Learner")
            {
                return RedirectToAction("LearnerLogin", "Account");
            }

            var learner = await _context.Learners.SingleOrDefaultAsync(u => u.Email == email);
            if (learner == null)
            {
                return RedirectToAction("LearnerLogin", "Account");
            }

            var learnerId = learner.LearnerId;

            var notifications = await _context.Notifications
                .FromSqlRaw("EXEC ViewNot @LearnerID = {0}", learnerId)
                .ToListAsync();

            return View(notifications);
        }

        public async Task<IActionResult> QuestMembers()
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

            var participants = await _questRepository.GetQuestMembersAsync(learner.LearnerId);
            return View(participants);
        }

        [HttpGet]
        public IActionResult AddGoal()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddGoal(LearningGoal model)
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

            if (ModelState.IsValid)
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var query =
                        "EXEC AddGoal @LearnerID, @GoalID, @ObjectiveType, @ProgressStatus, @GoalDescription, @TimeBound";
                    var command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@LearnerID", learner.LearnerId);
                    command.Parameters.AddWithValue("@GoalID", model.GoalId);
                    command.Parameters.AddWithValue("@ObjectiveType", model.ObjectiveType ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ProgressStatus", model.ProgressStatus ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@GoalDescription", model.GoalDescription ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@TimeBound",
                        model.TimeBound.HasValue ? (object)model.TimeBound.Value : DBNull.Value);

                    connection.Open();
                    await command.ExecuteNonQueryAsync();
                }

                return RedirectToAction("LearnerProfile", "Account");
            }

            return View(model);
        }

        public async Task<IActionResult> TrackProgress()
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
            var progressData = new List<QuestProgressViewModel>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("QuestProgress", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@LearnerID", learnerId);

                connection.Open();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        progressData.Add(new QuestProgressViewModel
                        {
                            QuestID = reader.GetInt32(0),
                            QuestTitle = reader.GetString(1),
                            QuestDescription = reader.GetString(2),
                            QuestStatus = reader.GetString(3),
                            BadgeID = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4),
                            BadgeTitle = reader.IsDBNull(5) ? null : reader.GetString(5),
                            BadgeDescription = reader.IsDBNull(6) ? null : reader.GetString(6),
                            BadgeStatus = reader.GetString(7)
                        });
                    }
                }
            }

            return View(progressData);
        }

        [HttpGet]
        public IActionResult ShareFeedback()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ShareFeedback(EmotionalFeedbackViewModel model)
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

            // Check if the ActivityID exists in the LearningActivity table
            var activityExists = await _context.LearningActivities.AnyAsync(a => a.ActivityId == model.ActivityID);
            if (!activityExists)
            {
                ModelState.AddModelError(string.Empty, "The specified Activity ID does not exist.");
                return View(model);
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("ActivityEmotionalFeedback", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@ActivityID", model.ActivityID);
                command.Parameters.AddWithValue("@LearnerID", learner.LearnerId);
                command.Parameters.AddWithValue("@timestamp", DateTime.Now);
                command.Parameters.AddWithValue("@emotionalstate", model.EmotionalState);

                connection.Open();
                await command.ExecuteNonQueryAsync();
            }

            return RedirectToAction("LearnerProfile", "Account");
        }
    }
}