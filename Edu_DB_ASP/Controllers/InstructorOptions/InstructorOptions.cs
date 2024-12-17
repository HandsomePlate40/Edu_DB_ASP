using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Edu_DB_ASP.Models; // Add this to include the models
using System.Linq;
using Microsoft.EntityFrameworkCore; // Add this to use LINQ

namespace Edu_DB_ASP.Controllers.InstructorOptions
{
    public class InstructorOptions : Controller
    {
        private readonly string _connectionString;
        private readonly EduDbContext _context; // Add this to define the context

        public InstructorOptions(IConfiguration configuration, EduDbContext context) // Add context to the constructor
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _context = context; // Initialize the context
        }

        [HttpGet]
        public IActionResult AddCollabQuest()
        {
            if (HttpContext.Session.GetString("UserRole") != "Instructor")
            {
                return RedirectToAction("LearnerLogin", "Account");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCollabQuest(CollaborativeQuestViewModel model)
        {
            if (HttpContext.Session.GetString("UserRole") != "Instructor")
            {
                return RedirectToAction("LearnerLogin", "Account");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    using (var connection = new SqlConnection(_connectionString))
                    {
                        var query =
                            "EXEC CollaborativeQuest @difficulty_level, @criteria, @description, @title, @Maxnumparticipants, @deadline";
                        var command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@difficulty_level", model.DifficultyLevel);
                        command.Parameters.AddWithValue("@criteria", model.Criteria);
                        command.Parameters.AddWithValue("@description", model.Description);
                        command.Parameters.AddWithValue("@title", model.Title);
                        command.Parameters.AddWithValue("@Maxnumparticipants", model.MaxParticipants);
                        command.Parameters.AddWithValue("@deadline", model.Deadline);

                        connection.Open();
                        await command.ExecuteNonQueryAsync();
                    }

                    TempData["SuccessMessage"] = "Collaborative quest added successfully.";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"An error occurred while adding the collaborative quest: {ex.Message}";
                }

                return RedirectToAction("AddCollabQuest", "InstructorOptions");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult AddDiscussion()
        {
            if (HttpContext.Session.GetString("UserRole") != "Instructor")
            {
                return RedirectToAction("LearnerLogin", "Account");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddDiscussion(int moduleId, int courseId, string title, string description)
        {
            if (HttpContext.Session.GetString("UserRole") != "Instructor")
            {
                return RedirectToAction("LearnerLogin", "Account");
            }

            string confirmationMessage = string.Empty;

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("CreateDiscussion", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@ModuleID", moduleId);
                command.Parameters.AddWithValue("@CourseID", courseId);
                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@Description", description);

                var outputParam = new SqlParameter("@ConfirmationMessage", System.Data.SqlDbType.VarChar, 100)
                {
                    Direction = System.Data.ParameterDirection.Output
                };
                command.Parameters.Add(outputParam);

                connection.Open();
                await command.ExecuteNonQueryAsync();
                confirmationMessage = outputParam.Value.ToString();
            }

            ViewBag.ConfirmationMessage = confirmationMessage;
            return View();
        }

        public IActionResult DeadlineUpdate()
        {
            if (HttpContext.Session.GetString("UserRole") != "Instructor")
            {
                return RedirectToAction("LearnerLogin", "Account");
            }

            var collaborativeQuests = _context.CollaborativeQuests.Include(q => q.Quest).ToList();
            return View(collaborativeQuests);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDeadline(int QuestID, DateTime Deadline)
        {
            if (HttpContext.Session.GetString("UserRole") != "Instructor")
            {
                return RedirectToAction("LearnerLogin", "Account");
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("DeadlineUpdate", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@QuestID", QuestID);
                command.Parameters.AddWithValue("@deadline", Deadline);

                connection.Open();
                await command.ExecuteNonQueryAsync();
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public IActionResult AddLearningPath()
        {
            if (HttpContext.Session.GetString("UserRole") != "Instructor")
            {
                return RedirectToAction("LearnerLogin", "Account");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddLearningPath(LearningPathViewModel model)
        {
            if (HttpContext.Session.GetString("UserRole") != "Instructor")
            {
                return RedirectToAction("LearnerLogin", "Account");
            }

            if (ModelState.IsValid)
            {
                // Check if the GoalID exists in the LearningGoal table
                var goalExists = await _context.LearningGoals.AnyAsync(g => g.GoalId == model.GoalID);
                if (!goalExists)
                {
                    ModelState.AddModelError("GoalID", "The specified GoalID does not exist.");
                    return View(model);
                }

                var sql =
                    "EXEC NewPath @LearnerID, @ProfileID, @completion_status, @custom_content, @adaptiverules, @GoalID";
                var parameters = new[]
                {
                    new SqlParameter("@LearnerID", model.LearnerID),
                    new SqlParameter("@ProfileID", model.ProfileID),
                    new SqlParameter("@completion_status", model.CompletionStatus),
                    new SqlParameter("@custom_content", model.CustomContent),
                    new SqlParameter("@adaptiverules", model.AdaptiveRules),
                    new SqlParameter("@GoalID", model.GoalID)
                };

                try
                {
                    await _context.Database.ExecuteSqlRawAsync(sql, parameters);
                    TempData["SuccessMessage"] = "Learning path added successfully.";
                }
                catch (SqlException ex)
                {
                    ModelState.AddModelError(string.Empty,
                        $"An error occurred while adding the learning path: {ex.Message}");
                    return View(model);
                }

                return RedirectToAction("AddLearningPath", "InstructorOptions");
            }

            return View(model);
        }

        public async Task<IActionResult> DeleteQuestsByCriteria(string criteria)
        {
            if (string.IsNullOrEmpty(criteria))
            {
                return BadRequest("Criteria cannot be null or empty.");
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Delete related records from the Partake table
                    var deletePartakeSql =
                        "DELETE FROM Partake WHERE QuestID IN (SELECT QuestID FROM Quests WHERE Criteria = @Criteria)";
                    var partakeParameters = new SqlParameter("@Criteria", criteria);
                    await _context.Database.ExecuteSqlRawAsync(deletePartakeSql, partakeParameters);

                    // Delete records from the Quests table
                    var deleteQuestsSql = "DELETE FROM Quests WHERE Criteria = @Criteria";
                    await _context.Database.ExecuteSqlRawAsync(deleteQuestsSql, partakeParameters);

                    await transaction.CommitAsync();

                    return Json(new
                        { success = true, message = $"All quests with the criteria '{criteria}' have been deleted." });
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    return Json(new { success = false, message = "An error occurred while deleting the quests." });
                }
            }
        }
        
        public IActionResult AddAchievement()
        {
            if (HttpContext.Session.GetString("UserRole") != "Instructor")
            {
                return RedirectToAction("LearnerLogin", "Account");
            }

            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> AddAchievement(AchievementViewModel model)
        {
            if (HttpContext.Session.GetString("UserRole") != "Instructor")
            {
                return RedirectToAction("LearnerLogin", "Account");
            }

            if (!ModelState.IsValid)
            {
                // Log ModelState errors
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(model);
            }

            var sql = "EXEC NewAchievement @LearnerID, @BadgeID, @AchievmentDescription, @type";
            var parameters = new[]
            {
                new SqlParameter("@LearnerID", model.LearnerID),
                new SqlParameter("@BadgeID", model.BadgeID),
                new SqlParameter("@AchievmentDescription", model.AchievmentDescription),
                new SqlParameter("@type", model.Type)
            };

            try
            {
                await _context.Database.ExecuteSqlRawAsync(sql, parameters);
                TempData["SuccessMessage"] = "Achievement added successfully.";
                Console.WriteLine("Achievement added successfully.");
            }
            catch (SqlException ex)
            {
                var refinedErrorMessage = $"An error occurred while adding the achievement: {ex.Message}";
                ModelState.AddModelError(string.Empty, refinedErrorMessage);
                Console.WriteLine(refinedErrorMessage);
                return View(model);
            }

            return RedirectToAction("AddAchievement", "InstructorOptions");
        }
    }
}