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
        
        public void AddAssessment(string Title, string description, string criteria, decimal weightage, decimal max,int marks, int pass )
        {
            var assessment = new Assessment
            {
                Title = Title,
                AssessmentDescription = description,
                GradingCriteria = criteria,
                Weightage = weightage,
                MaxScore = max,
                TotalMarks = marks,
                PassingMarks = pass,
                
                
            };
            _context.Assessments.Add(assessment);
            _context.SaveChangesAsync();
           
        }
        public void AddAssessment(string Title, string description, string criteria, decimal weightage, decimal max, int marks, int pass, int module)
        {
            var assessment = new Assessment
            {
                Title = Title,
                AssessmentDescription = description,
                GradingCriteria = criteria,
                Weightage = weightage,
                MaxScore = max,
                TotalMarks = marks,
                PassingMarks = pass,
                ModuleId = module

            };
            _context.Assessments.Add(assessment);
            _context.SaveChangesAsync();

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
            var assessment = await _context.Assessments
                .FirstOrDefaultAsync(a => a.AssessmentId == model.AssessmentId);

            if (assessment == null)
            {
                return NotFound($"The LearnerID/Assessment title is incorrect. Please Re-enter");
            }

            var learnerIdP = new SqlParameter("@LearnerID", model.LearnerId);
            var assessmentId = new SqlParameter("@AssessmentID", assessment.AssessmentId);
            var scoreP = new SqlParameter("@Score", model.score);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC GradeUpdate @LearnerID, @AssessmentID, @points",
                learnerIdP,
                assessmentId,
                scoreP
            );
            _context.SaveChangesAsync();

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
                AssessmentName = ta.Assessment.Title,
                course = ta.Assessment.Module.Course.Title,
                score = ta.ScoredPoints + "/" + ta.Assessment.MaxScore
                

            }).ToList();

            return View(learnerAssessmentList);
        }
        [HttpPost]
        public async Task<IActionResult> LearnerAssessmentView(LearnerAssessmentFilter filter)
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            var learnerId = await _context.Learners
                .Where(l => l.Email == userEmail)
                .Select(l => l.LearnerId)
                .FirstOrDefaultAsync();

            if (!string.IsNullOrEmpty(filter.Title))
            {
                var filteredList = await LearnerGetTakenAssessments(learnerId, filter.Title);
                var assessmentList = filteredList.Select(ta => new
                {
                    AssessmentId = ta.AssessmentId,
                    AssessmentName = ta.Assessment.Title,
                    Course = ta.Assessment.Module.Course.Title,
                    Score = ta.ScoredPoints + "/" + ta.Assessment.MaxScore
                }).ToList();

                return View(assessmentList); 
            }

            var defaultAssessmentList = await LearnerGetTakenAssessments(learnerId);
            var learnerAssessmentList = defaultAssessmentList.Select(ta => new
            {
                AssessmentId = ta.AssessmentId,
                AssessmentName = ta.Assessment.Title,
                Course = ta.Assessment.Module.Course.Title,
                Score = ta.ScoredPoints + "/" + ta.Assessment.MaxScore
            }).ToList();

            return View(learnerAssessmentList); 
        }

        [HttpGet]
        public async Task<IActionResult> InstructorAssessmentView()   
        {
            var assessmentlist = await InstructorGetTakenAssessments(null);
            var learnerAssessmentList = assessmentlist.Select(ta => new
            {
                LearnerId = ta.LearnerId,
                AssessmentName = ta.Assessment.Title,
                course = ta.Assessment.Module.Course.Title,
                score = ta.ScoredPoints+"/"+ta.Assessment.MaxScore
            }).ToList();

            return View(learnerAssessmentList);
        }
        [HttpPost]
        public async Task<IActionResult> InstructorAssessmentView(InstructorAssessmentFilter filter)
        {

            var assessmentlist = await InstructorGetTakenAssessments(filter.LearnerId,filter.Title);
            var learnerAssessmentList = assessmentlist.Select(ta => new
            {
                LearnerId = ta.LearnerId,
                AssessmentName = ta.Assessment.Title,
                score = ta.ScoredPoints + "/" + ta.Assessment.MaxScore
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
            if (ModelState.IsValid)
            {
                try
                {
                   if(model.ModuleId ==null)
                    {
                        AddAssessment(model.Title, model.AssessmentDescription, model.GradingCriteria, model.Weightage, model.MaxScore, model.TotalMarks, model.PassingMarks);
                    }
                    else
                    {
                        AddAssessment(model.Title, model.AssessmentDescription, model.GradingCriteria, model.Weightage, model.MaxScore, model.TotalMarks, model.PassingMarks, model.ModuleId);
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction("InstructorAssessmentView");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> TopView()
        {
            var assessments = await _context.Assessments.FromSqlRaw("EXEC Highestgrade").ToListAsync();
            var fassessments = assessments.Select(a => new
            {
                ID =a.AssessmentId,
                title =a.Title,
                max =a.MaxScore,
                course =a.Module.Course.CourseId
            }
            );
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
            if(ModelState.IsValid)
            {
                try
                {
                    if (model.Message == null)
                    {
                        var date = DateTime.Now.ToString("MM/dd/yyyy");
                        await _context.Database.ExecuteSqlRawAsync(
                         "EXEC AssessmentNot @timestamp, @message, @urgencylevel, @LearnerID",
                         new SqlParameter("@timestamp", date),
                         new SqlParameter("@message", ""),
                         new SqlParameter("@urgencylevel", model.UrgencyLevel),
                         new SqlParameter("@LearnerID", model.LearnerId)
                         
                     );
                    }
                    else
                    {
                        var date = DateTime.Now.ToString("MM/dd/yyyy");
                        await _context.Database.ExecuteSqlRawAsync(
                        "EXEC AssessmentNot @timestamp, @message, @urgencylevel, @LearnerID",
                        new SqlParameter("@timestamp", date),
                        new SqlParameter("@message", model.Message),
                        new SqlParameter("@urgencylevel", model.UrgencyLevel),
                        new SqlParameter("@LearnerID", model.LearnerId)
    );
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction("InstructorAssessmentView");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                }
            }

            return View(model);
        }



    }
 }



