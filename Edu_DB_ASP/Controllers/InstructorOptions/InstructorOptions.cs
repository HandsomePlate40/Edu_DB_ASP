using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Edu_DB_ASP.Controllers.InstructorOptions
{
    public class InstructorOptions : Controller
    {
        private readonly string _connectionString;

        public InstructorOptions(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
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
                using (var connection = new SqlConnection(_connectionString))
                {
                    var query = "EXEC CollaborativeQuest @difficulty_level, @criteria, @description, @title, @Maxnumparticipants, @deadline";
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
    }
}