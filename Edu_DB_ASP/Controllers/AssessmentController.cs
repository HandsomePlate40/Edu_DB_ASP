using Edu_DB_ASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Edu_DB_ASP.Controllers.Assessments
{
    public class AssessmentController : Controller
    {
        private readonly EduDbContext _context;

        public AssessmentController(EduDbContext context)
        {
            _context = context;
        }
        
   


        public async Task<List<TakenAssessment>> LearnerGetTakenAssessments(int learnerId, string title = null )
        {
            var assessments = await _context.TakenAssessments
                   .Where(a => a.LearnerId == learnerId )
                   .ToListAsync();
            if (title!=null && title != "") 
            {
                 assessments = await _context.TakenAssessments
                    .Where(ta => ta.LearnerId == learnerId && ta.Assessment.Title == title)
                    .ToListAsync();
                
            }
            return assessments; 
        }
        
        
        [HttpGet]
        public  IActionResult UpdateScore()
        {
            return View();
        }
        
        
        [HttpPost]
        public async Task<IActionResult> UpdateScore(AssessmentUpdateModel model)
        {
            // Find the assessment by title
            var assessment = await _context.Assessments
                .FirstOrDefaultAsync(a => a.AssessmentId == model.AssessmentId);
            var learner = await _context.Learners
                .FirstOrDefaultAsync(a => a.LearnerId == model.LearnerId);

            if (assessment == null)
            {
                return NotFound($"The Assessment isn't available. Please re-enter.");
            }
            if (learner == null)
            {
                return NotFound($"There isn't a learner with such ID. Please re-enter.");
            }

            var takenAssessment = await _context.TakenAssessments
                .FirstOrDefaultAsync(ta => ta.LearnerId == model.LearnerId && ta.AssessmentId == assessment.AssessmentId);

            if (takenAssessment == null)
            {
                var newTakenAssessment = new TakenAssessment
                {
                    LearnerId = model.LearnerId,
                    AssessmentId = assessment.AssessmentId,
                    ScoredPoints = model.score 
                };

                _context.TakenAssessments.Add(newTakenAssessment);
            }
            else
            {
                takenAssessment.ScoredPoints = model.score;
                _context.TakenAssessments.Update(takenAssessment);
            }

            var learnerIdP = new SqlParameter("@LearnerID", model.LearnerId);
            var assessmentId = new SqlParameter("@AssessmentID", assessment.AssessmentId);
            var scoreP = new SqlParameter("@Score", model.score);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC GradeUpdate @LearnerID, @AssessmentID, @Score",
                learnerIdP,
                assessmentId,
                scoreP
            );

            await _context.SaveChangesAsync(); 

            return RedirectToAction("InstructorAssessmentView"); 
        }

        public async Task<List<TakenAssessment>> InstructorGetTakenAssessments(int? learnerId , string title = null)
        {
            var assessments = await _context.TakenAssessments.ToListAsync();

            if (title!=null && title != "" && learnerId !=null) 
            {
                 assessments = await _context.TakenAssessments
                    .Where(ta => ta.LearnerId == learnerId && ta.Assessment.Title == title)
                    .ToListAsync();
                
            }else if(title!=null && title !=""&& learnerId == null)
            {
                   assessments = await _context.TakenAssessments
                   .Where(a => a.Assessment.Title == title)
                   .ToListAsync();
            }else if(title == null && learnerId != null)
            {
                assessments = await _context.TakenAssessments
                   .Where(a => a.LearnerId == learnerId)
                   .ToListAsync();
            }

            return assessments;
        }


        [HttpGet]
        public async Task<IActionResult> LearnerAssessmentView()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");

            var learnerId = _context.Learners
                .Where(l => l.Email == userEmail)
                .Select(l => l.LearnerId)
                .FirstOrDefault();
            var assessmentlist = await LearnerGetTakenAssessments(learnerId);
            var learnerAssessmentList = assessmentlist.Select(ta => new
            {
                AssessmentId = ta.AssessmentId,
                AssessmentName = ta.Assessment?.Title ?? "N/A",
                score = ta.ScoredPoints + "/" + (ta.Assessment?.TotalMarks ?? 0)
            }).ToList();

            return View(learnerAssessmentList);
        }

        [HttpPost]
        public async Task<IActionResult> LearnerAssessmentView(LearnerAssessmentFilter filter)
        {
            var assessment = await _context.Assessments
                .FirstOrDefaultAsync(a => a.Title == filter.Title);

            if (assessment == null)
            {
                return NotFound($"The Assessment isn't available. Please re-enter.");
            }

            var userEmail = HttpContext.Session.GetString("UserEmail");
            int learnerId = await _context.Learners
                .Where(l => l.Email == userEmail)
                .Select(l => l.LearnerId)
                .FirstOrDefaultAsync();
            {
                var filteredList = await LearnerGetTakenAssessments(learnerId, filter.Title);
                var learnerAssessmentList = filteredList.Select(ta => new
                {
                    AssessmentId = ta.AssessmentId,
                    AssessmentName = ta.Assessment.Title,
                    Course = ta.Assessment.Module.Course.Title,
                    Score = ta.ScoredPoints + "/" + ta.Assessment.TotalMarks
                }).ToList();

                return View(learnerAssessmentList);


            }
        }

        [HttpGet]
        public async Task<IActionResult> InstructorAssessmentView(int? learnerId = null, string title = null)
        {
            var assessmentlist = await InstructorGetTakenAssessments(learnerId, title);
            var learnerAssessmentList = assessmentlist.Select(ta => new
            {
                LearnerId = ta.LearnerId,
                AssessmentName = ta.Assessment?.Title ?? "N/A",
                score = ta.ScoredPoints + "/" + (ta.Assessment?.TotalMarks ?? 0)
            }).ToList();

            return View(learnerAssessmentList);
        }
        
        [HttpPost]
        public async Task<IActionResult> InstructorAssessmentView(InstructorAssessmentFilter filter)
        {
            var assessment = await _context.Assessments
                .FirstOrDefaultAsync(a => a.Title == filter.Title);
            var learner = await _context.Learners
                .FirstOrDefaultAsync(a => a.LearnerId == filter.LearnerId);

            if (assessment == null)
            {
                return NotFound($"The Assessment isn't available. Please re-enter.");
            }
            if (learner == null)
            {
                return NotFound($"There isn't a learner with such ID. Please re-enter.");
            }

            var assessmentlist = await InstructorGetTakenAssessments(filter.LearnerId,filter.Title);
            var learnerAssessmentList = assessmentlist.Select(ta => new
            {
                LearnerId = ta.LearnerId,
                AssessmentName = ta.Assessment.Title,
                score = ta.ScoredPoints + "/" + ta.Assessment.TotalMarks
            }).ToList();

            return View(learnerAssessmentList);
        }

        [HttpGet]
        public IActionResult AssessmentAnalysis()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AssessmentAnalysis(int? assessmentId)
        {
            if(assessmentId == null)
            {
                var Analysis = await (from a in _context.Assessments
                                                join ta in _context.TakenAssessments on a.AssessmentId equals ta.AssessmentId
                                                group ta by new
                                                {
                                                    a.AssessmentId,
                                                    a.Title
                                                } into g
                                                select new
                                                {
                                                    AssessmentId = g.Key.AssessmentId,
                                                    AssessmentTitle = g.Key.Title,
                                                    HighestScore = g.Max(x => x.ScoredPoints),
                                                    AverageScore = g.Average(x => x.ScoredPoints)
                                                }).ToListAsync();

                return View(Analysis);
            }
            
            
            
            var assessmentAnalysis = await (from a in _context.Assessments
                                            join ta in _context.TakenAssessments on a.AssessmentId equals ta.AssessmentId
                                            where a.AssessmentId == assessmentId 
                                            group ta by new
                                            {
                                                a.AssessmentId,
                                                a.Title
                                            } into g
                                            select new
                                            {
                                                AssessmentId = g.Key.AssessmentId,
                                                AssessmentTitle = g.Key.Title,
                                                HighestScore = g.Max(x => x.ScoredPoints), 
                                                AverageScore = g.Average(x => x.ScoredPoints) 
                                            }).ToListAsync();

            return View(assessmentAnalysis);
        }



        [HttpGet]
        public IActionResult AddAssessmentView()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAssessmentView(AddAssessmentModel model)
        {
         
                try
                {
                        var assessment = new Assessment
                        {
                            AssessmentId = model.AssessmentId,
                            Title = model.Title,
                            AssessmentDescription = model.AssessmentDescription,
                            GradingCriteria = model.GradingCriteria,
                            Weightage = model.Weightage,
                            MaxScore = 0,
                            TotalMarks = model.TotalMarks,
                            PassingMarks = model.PassingMarks,
                            ModuleId = model.ModuleId


                        };
                        _context.Assessments.Add(assessment);
                        
                    await _context.SaveChangesAsync();
                    return RedirectToAction("InstructorAssessmentView");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                }
            

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> TopView()
        {
            var assessments = await _context.Assessments.FromSqlRaw("EXEC Highestgrade").ToListAsync();
            var fassessments = assessments.Select(a => new
            {
                ID = a.AssessmentId,
                title = a.Title ?? "N/A",
                max = a.MaxScore,
                course = a.Module?.Course?.CourseId ?? 0
            }).ToList();
            return View(fassessments);
        }
        
        
        [HttpGet]
        public IActionResult LearnerReset()
        {
            return RedirectToAction("LearnerAssessmentView");
        }

        [HttpGet]
        public IActionResult InstructorReset()
        {
            return RedirectToAction("InstructorAssessmentView");
        }

        [HttpGet]
        public IActionResult AssessmentNoti()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AssessmentNoti(AssessmentNotiModel model)
        {
            try
            {
                var date = DateTime.Now.ToString("MM/dd/yyyy");
                var result = await _context.Database.ExecuteSqlRawAsync(
                    "EXEC AssessmentNot @timestamp, @message, @urgencylevel, @LearnerID",
                    new SqlParameter("@timestamp", date),
                    new SqlParameter("@message", model.Message),
                    new SqlParameter("@urgencylevel", model.UrgencyLevel),
                    new SqlParameter("@LearnerID", model.LearnerId)
                );

                if (result == 0)
                {
                    ModelState.AddModelError("", "No rows were affected. Please check the stored procedure.");
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("InstructorAssessmentView");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                // Optionally log the exception here
            }

            return View(model);
        }
        
        

    }
 }



