using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edu_DB_ASP.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Badges",
                columns: table => new
                {
                    BadgeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BadgeTitle = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    BadgeDescription = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    CriteriaToUnlock = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    PointsValue = table.Column<decimal>(type: "decimal(5,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Badges__1918237CBBA0B7BA", x => x.BadgeID);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    CourseID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    CourseDescription = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    DifficultyLevel = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Prerequisites = table.Column<string>(type: "varchar(212)", unicode: false, maxLength: 212, nullable: true),
                    CreditPoints = table.Column<decimal>(type: "decimal(4,2)", nullable: true),
                    LearningObjectives = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Courses__C92D7187B5609625", x => x.CourseID);
                });

            migrationBuilder.CreateTable(
                name: "Leaderboard",
                columns: table => new
                {
                    LeaderboardID = table.Column<int>(type: "int", nullable: false),
                    TotalPoints = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Season = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Leaderbo__B358A1E686B80F17", x => x.LeaderboardID);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageBody = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UrgencyLevel = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ReadStatus = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Notifica__20CF2E321C986044", x => x.NotificationID);
                });

            migrationBuilder.CreateTable(
                name: "Quests",
                columns: table => new
                {
                    QuestID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    QuestDescription = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    DifficultyLevel = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    QuestType = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Criteria = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Quests__B6619ACB885D6696", x => x.QuestID);
                });

            migrationBuilder.CreateTable(
                name: "Reward",
                columns: table => new
                {
                    RewardID = table.Column<int>(type: "int", nullable: false),
                    RewardType = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    RewardValue = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    RewardDescription = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Reward__82501599E3A34849", x => x.RewardID);
                });

            migrationBuilder.CreateTable(
                name: "Survey",
                columns: table => new
                {
                    SurveyID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    ResponseTimestamp = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Survey__A5481F9DA746892B", x => x.SurveyID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "ContentLibrary",
                columns: table => new
                {
                    ContentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    ContentType = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    ContentURL = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    Keywords = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    DifficultyLevel = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    LearningObjectives = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    CourseID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ContentL__2907A87EC282FE42", x => x.ContentID);
                    table.ForeignKey(
                        name: "FK__ContentLi__Cours__1332DBDC",
                        column: x => x.CourseID,
                        principalTable: "Courses",
                        principalColumn: "CourseID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    ModuleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    ContentType = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    ContentURL = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    ModuleDifficulty = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CourseID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Modules__2B7477876BE6F7D6", x => x.ModuleID);
                    table.ForeignKey(
                        name: "FK__Modules__CourseI__5070F446",
                        column: x => x.CourseID,
                        principalTable: "Courses",
                        principalColumn: "CourseID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Collaborative_Quests",
                columns: table => new
                {
                    QuestID = table.Column<int>(type: "int", nullable: false),
                    Max_Participants = table.Column<int>(type: "int", nullable: true),
                    DeadLine = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Collabor__B6619ACBF03D3510", x => x.QuestID);
                    table.ForeignKey(
                        name: "FK__Collabora__Quest__7D439ABD",
                        column: x => x.QuestID,
                        principalTable: "Quests",
                        principalColumn: "QuestID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestReward",
                columns: table => new
                {
                    QuestID = table.Column<int>(type: "int", nullable: false),
                    Rewards = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__QuestRew__D749EC5578916AF7", x => new { x.QuestID, x.Rewards });
                    table.ForeignKey(
                        name: "FK__QuestRewa__Quest__00200768",
                        column: x => x.QuestID,
                        principalTable: "Quests",
                        principalColumn: "QuestID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Skill_Mastery_Quests",
                columns: table => new
                {
                    QuestID = table.Column<int>(type: "int", nullable: false),
                    Skills = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Skill_Ma__B6619ACBBB199C12", x => x.QuestID);
                    table.ForeignKey(
                        name: "FK__Skill_Mas__Quest__7A672E12",
                        column: x => x.QuestID,
                        principalTable: "Quests",
                        principalColumn: "QuestID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Grants",
                columns: table => new
                {
                    QuestID = table.Column<int>(type: "int", nullable: false),
                    RewardID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Grants__3E449B92A94CEF25", x => new { x.QuestID, x.RewardID });
                    table.ForeignKey(
                        name: "FK__Grants__QuestID__2EDAF651",
                        column: x => x.QuestID,
                        principalTable: "Quests",
                        principalColumn: "QuestID");
                    table.ForeignKey(
                        name: "FK__Grants__RewardID__2FCF1A8A",
                        column: x => x.RewardID,
                        principalTable: "Reward",
                        principalColumn: "RewardID");
                });

            migrationBuilder.CreateTable(
                name: "SurveyQuestions",
                columns: table => new
                {
                    SurveyID = table.Column<int>(type: "int", nullable: false),
                    Question = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SurveyQu__23FB983B6909533D", x => new { x.SurveyID, x.Question });
                    table.ForeignKey(
                        name: "FK__SurveyQue__Surve__208CD6FA",
                        column: x => x.SurveyID,
                        principalTable: "Survey",
                        principalColumn: "SurveyID");
                });

            migrationBuilder.CreateTable(
                name: "Instructor",
                columns: table => new
                {
                    InstructorID = table.Column<int>(type: "int", nullable: false),
                    InstructorName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Qualifications = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Instruct__9D010B7BDE0D9400", x => x.InstructorID);
                    table.ForeignKey(
                        name: "FK_Instructor_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Learner",
                columns: table => new
                {
                    LearnerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    first_name = table.Column<string>(type: "varchar(35)", unicode: false, maxLength: 35, nullable: true),
                    last_name = table.Column<string>(type: "varchar(35)", unicode: false, maxLength: 35, nullable: true),
                    birthdate = table.Column<DateTime>(type: "datetime", nullable: true),
                    age = table.Column<int>(type: "int", nullable: true, computedColumnSql: "(datepart(year,getdate())-datepart(year,[birthdate]))", stored: false),
                    gender = table.Column<string>(type: "varchar(6)", unicode: false, maxLength: 6, nullable: true),
                    CountryOfOrigin = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    cultural_background = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    PersonalityTraits = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    EmotionalProfile = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    PhysicalHealth = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    MentalHealth = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    ExperienceLevel = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Learner__67ABFCFA1EEFDCD8", x => x.LearnerID);
                    table.ForeignKey(
                        name: "FK_Learner_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Assessments",
                columns: table => new
                {
                    AssessmentID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    AssessmentDescription = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    GradingCriteria = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    Weightage = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    MaxScore = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    TotalMarks = table.Column<int>(type: "int", nullable: true),
                    PassingMarks = table.Column<int>(type: "int", nullable: true),
                    ModuleID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Assessme__3D2BF83E5A3CB530", x => x.AssessmentID);
                    table.ForeignKey(
                        name: "FK__Assessmen__Modul__07C12930",
                        column: x => x.ModuleID,
                        principalTable: "Modules",
                        principalColumn: "ModuleID");
                });

            migrationBuilder.CreateTable(
                name: "DiscussionForum",
                columns: table => new
                {
                    ForumID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    LastActiveTimestamp = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    ModuleID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Discussi__E210AC4F7B9376C5", x => x.ForumID);
                    table.ForeignKey(
                        name: "FK__Discussio__Modul__17036CC0",
                        column: x => x.ModuleID,
                        principalTable: "Modules",
                        principalColumn: "ModuleID");
                });

            migrationBuilder.CreateTable(
                name: "LearningActivity",
                columns: table => new
                {
                    ActivityID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityType = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    MaxPoints = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    ModuleID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Learning__45F4A7F148FC20D3", x => x.ActivityID);
                    table.ForeignKey(
                        name: "FK__LearningA__Modul__5629CD9C",
                        column: x => x.ModuleID,
                        principalTable: "Modules",
                        principalColumn: "ModuleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TargetTraits",
                columns: table => new
                {
                    ModuleID = table.Column<int>(type: "int", nullable: false),
                    Trait = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TargetTr__B51B9AB7C8818EC9", x => new { x.ModuleID, x.Trait });
                    table.ForeignKey(
                        name: "FK__TargetTra__Modul__534D60F1",
                        column: x => x.ModuleID,
                        principalTable: "Modules",
                        principalColumn: "ModuleID");
                });

            migrationBuilder.CreateTable(
                name: "Expertise",
                columns: table => new
                {
                    InstructorID = table.Column<int>(type: "int", nullable: false),
                    ExpertiseArea = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Expertis__2B5DADA429D76C0C", x => new { x.InstructorID, x.ExpertiseArea });
                    table.ForeignKey(
                        name: "FK__Expertise__Instr__10566F31",
                        column: x => x.InstructorID,
                        principalTable: "Instructor",
                        principalColumn: "InstructorID");
                });

            migrationBuilder.CreateTable(
                name: "Teach",
                columns: table => new
                {
                    InstructorID = table.Column<int>(type: "int", nullable: false),
                    CourseID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Teach__F193DC63B5052AB2", x => new { x.InstructorID, x.CourseID });
                    table.ForeignKey(
                        name: "FK__Teach__CourseID__540C7B00",
                        column: x => x.CourseID,
                        principalTable: "Courses",
                        principalColumn: "CourseID");
                    table.ForeignKey(
                        name: "FK__Teach__Instructo__531856C7",
                        column: x => x.InstructorID,
                        principalTable: "Instructor",
                        principalColumn: "InstructorID");
                });

            migrationBuilder.CreateTable(
                name: "Achievement",
                columns: table => new
                {
                    AchievementID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AchievementType = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    DateEarned = table.Column<DateOnly>(type: "date", nullable: true),
                    LearnerID = table.Column<int>(type: "int", nullable: false),
                    BadgeID = table.Column<int>(type: "int", nullable: true),
                    AchievementDescription = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Achievem__276330E0200B5C44", x => x.AchievementID);
                    table.ForeignKey(
                        name: "FK__Achieveme__Badge__628FA481",
                        column: x => x.BadgeID,
                        principalTable: "Badges",
                        principalColumn: "BadgeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__Achieveme__Learn__619B8048",
                        column: x => x.LearnerID,
                        principalTable: "Learner",
                        principalColumn: "LearnerID");
                });

            migrationBuilder.CreateTable(
                name: "CourseEnrollment",
                columns: table => new
                {
                    EnrollmentID = table.Column<int>(type: "int", nullable: false),
                    LearnerID = table.Column<int>(type: "int", nullable: false),
                    CourseID = table.Column<int>(type: "int", nullable: false),
                    EnrollmentStatus = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    EnrollmentDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CompletionDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CourseEn__4FDBE545C4B4E420", x => new { x.EnrollmentID, x.LearnerID, x.CourseID });
                    table.ForeignKey(
                        name: "FK__CourseEnr__Cours__5CD6CB2B",
                        column: x => x.CourseID,
                        principalTable: "Courses",
                        principalColumn: "CourseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__CourseEnr__Learn__5BE2A6F2",
                        column: x => x.LearnerID,
                        principalTable: "Learner",
                        principalColumn: "LearnerID");
                });

            migrationBuilder.CreateTable(
                name: "EmotionalFeedback",
                columns: table => new
                {
                    FeedbackID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LearnerID = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    EmotionalState = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Emotiona__6A4BEDF6C079427F", x => x.FeedbackID);
                    table.ForeignKey(
                        name: "FK__Emotional__Learn__75A278F5",
                        column: x => x.LearnerID,
                        principalTable: "Learner",
                        principalColumn: "LearnerID");
                });

            migrationBuilder.CreateTable(
                name: "LeaderboardRanks",
                columns: table => new
                {
                    LeaderboardID = table.Column<int>(type: "int", nullable: false),
                    LearnerID = table.Column<int>(type: "int", nullable: false),
                    LeaderboardRank = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Leaderbo__05221E2914A09FB2", x => new { x.LeaderboardID, x.LearnerID });
                    table.ForeignKey(
                        name: "FK__Leaderboa__Leade__6754599E",
                        column: x => x.LeaderboardID,
                        principalTable: "Leaderboard",
                        principalColumn: "LeaderboardID");
                    table.ForeignKey(
                        name: "FK__Leaderboa__Learn__68487DD7",
                        column: x => x.LearnerID,
                        principalTable: "Learner",
                        principalColumn: "LearnerID");
                });

            migrationBuilder.CreateTable(
                name: "LeaderboardStudentsCourse",
                columns: table => new
                {
                    LearnerID = table.Column<int>(type: "int", nullable: false),
                    LeaderboardID = table.Column<int>(type: "int", nullable: false),
                    CourseID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Leaderbo__1C9E76E421B43CAB", x => new { x.LearnerID, x.LeaderboardID });
                    table.ForeignKey(
                        name: "FK__Leaderboa__Cours__6B24EA82",
                        column: x => x.CourseID,
                        principalTable: "Courses",
                        principalColumn: "CourseID");
                    table.ForeignKey(
                        name: "FK__Leaderboa__Leade__6D0D32F4",
                        column: x => x.LeaderboardID,
                        principalTable: "Leaderboard",
                        principalColumn: "LeaderboardID");
                    table.ForeignKey(
                        name: "FK__Leaderboa__Learn__6C190EBB",
                        column: x => x.LearnerID,
                        principalTable: "Learner",
                        principalColumn: "LearnerID");
                });

            migrationBuilder.CreateTable(
                name: "LearnerSkills",
                columns: table => new
                {
                    LearnerID = table.Column<int>(type: "int", nullable: false),
                    skill = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LearnerS__C45BDEA5E48E410D", x => new { x.LearnerID, x.skill });
                    table.ForeignKey(
                        name: "FK__LearnerSk__Learn__3A81B327",
                        column: x => x.LearnerID,
                        principalTable: "Learner",
                        principalColumn: "LearnerID");
                });

            migrationBuilder.CreateTable(
                name: "LearnersSurvey",
                columns: table => new
                {
                    LearnerID = table.Column<int>(type: "int", nullable: false),
                    SurveyID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Learners__ADFF7D039B217D05", x => new { x.LearnerID, x.SurveyID });
                    table.ForeignKey(
                        name: "FK__LearnersS__Learn__236943A5",
                        column: x => x.LearnerID,
                        principalTable: "Learner",
                        principalColumn: "LearnerID");
                    table.ForeignKey(
                        name: "FK__LearnersS__Surve__245D67DE",
                        column: x => x.SurveyID,
                        principalTable: "Survey",
                        principalColumn: "SurveyID");
                });

            migrationBuilder.CreateTable(
                name: "LearningGoal",
                columns: table => new
                {
                    GoalID = table.Column<int>(type: "int", nullable: false),
                    ObjectiveType = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    ProgressStatus = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    GoalDescription = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    TimeBound = table.Column<DateTime>(type: "datetime", nullable: true),
                    LearnerID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Learning__8A4FFF311BEAC4EC", x => x.GoalID);
                    table.ForeignKey(
                        name: "FK__LearningG__Learn__4316F928",
                        column: x => x.LearnerID,
                        principalTable: "Learner",
                        principalColumn: "LearnerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LearningPreferences",
                columns: table => new
                {
                    LearnerID = table.Column<int>(type: "int", nullable: false),
                    preference = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Learning__6032E158494B9BDA", x => new { x.LearnerID, x.preference });
                    table.ForeignKey(
                        name: "FK__LearningP__Learn__3D5E1FD2",
                        column: x => x.LearnerID,
                        principalTable: "Learner",
                        principalColumn: "LearnerID");
                });

            migrationBuilder.CreateTable(
                name: "Partake",
                columns: table => new
                {
                    QuestID = table.Column<int>(type: "int", nullable: false),
                    LearnerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Partake__001B2504CA672BC5", x => new { x.QuestID, x.LearnerID });
                    table.ForeignKey(
                        name: "FK__Partake__Learner__339FAB6E",
                        column: x => x.LearnerID,
                        principalTable: "Learner",
                        principalColumn: "LearnerID");
                    table.ForeignKey(
                        name: "FK__Partake__QuestID__32AB8735",
                        column: x => x.QuestID,
                        principalTable: "Quests",
                        principalColumn: "QuestID");
                });

            migrationBuilder.CreateTable(
                name: "Receives",
                columns: table => new
                {
                    LearnerID = table.Column<int>(type: "int", nullable: false),
                    NotificationID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Receives__55A70E193D54808C", x => new { x.LearnerID, x.NotificationID });
                    table.ForeignKey(
                        name: "FK__Receives__Learne__4A8310C6",
                        column: x => x.LearnerID,
                        principalTable: "Learner",
                        principalColumn: "LearnerID");
                    table.ForeignKey(
                        name: "FK__Receives__Notifi__4B7734FF",
                        column: x => x.NotificationID,
                        principalTable: "Notifications",
                        principalColumn: "NotificationID");
                });

            migrationBuilder.CreateTable(
                name: "SkillProgression",
                columns: table => new
                {
                    ProgressID = table.Column<int>(type: "int", nullable: false),
                    Specific_Skill = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    ProficiencyLevel = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    LearnerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SkillPro__BAE29C85DEE3B5F7", x => x.ProgressID);
                    table.ForeignKey(
                        name: "FK__SkillProg__Learn__04E4BC85",
                        column: x => x.LearnerID,
                        principalTable: "Learner",
                        principalColumn: "LearnerID");
                });

            migrationBuilder.CreateTable(
                name: "Takes",
                columns: table => new
                {
                    BadgeID = table.Column<int>(type: "int", nullable: false),
                    LearnerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Takes__AF629CB3EB07A3C2", x => new { x.BadgeID, x.LearnerID });
                    table.ForeignKey(
                        name: "FK__Takes__BadgeID__367C1819",
                        column: x => x.BadgeID,
                        principalTable: "Badges",
                        principalColumn: "BadgeID");
                    table.ForeignKey(
                        name: "FK__Takes__LearnerID__37703C52",
                        column: x => x.LearnerID,
                        principalTable: "Learner",
                        principalColumn: "LearnerID");
                });

            migrationBuilder.CreateTable(
                name: "Evaluates",
                columns: table => new
                {
                    CourseID = table.Column<int>(type: "int", nullable: false),
                    ModuleID = table.Column<int>(type: "int", nullable: false),
                    AssessmentID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Evaluate__94A71D07350C7833", x => new { x.CourseID, x.ModuleID, x.AssessmentID });
                    table.ForeignKey(
                        name: "FK__Evaluates__Asses__503BEA1C",
                        column: x => x.AssessmentID,
                        principalTable: "Assessments",
                        principalColumn: "AssessmentID");
                    table.ForeignKey(
                        name: "FK__Evaluates__Cours__4E53A1AA",
                        column: x => x.CourseID,
                        principalTable: "Courses",
                        principalColumn: "CourseID");
                    table.ForeignKey(
                        name: "FK__Evaluates__Modul__4F47C5E3",
                        column: x => x.ModuleID,
                        principalTable: "Modules",
                        principalColumn: "ModuleID");
                });

            migrationBuilder.CreateTable(
                name: "TakenAssessment",
                columns: table => new
                {
                    AssessmentID = table.Column<int>(type: "int", nullable: false),
                    LearnerID = table.Column<int>(type: "int", nullable: false),
                    ScoredPoints = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TakenAss__8B5147F11953F770", x => new { x.AssessmentID, x.LearnerID });
                    table.ForeignKey(
                        name: "FK__TakenAsse__Asses__0A9D95DB",
                        column: x => x.AssessmentID,
                        principalTable: "Assessments",
                        principalColumn: "AssessmentID");
                    table.ForeignKey(
                        name: "FK__TakenAsse__Learn__0B91BA14",
                        column: x => x.LearnerID,
                        principalTable: "Learner",
                        principalColumn: "LearnerID");
                });

            migrationBuilder.CreateTable(
                name: "Joins",
                columns: table => new
                {
                    LearnerID = table.Column<int>(type: "int", nullable: false),
                    ForumID = table.Column<int>(type: "int", nullable: false),
                    Post = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Joins__898AF63EBA3E703D", x => new { x.LearnerID, x.ForumID });
                    table.ForeignKey(
                        name: "FK__Joins__ForumID__1AD3FDA4",
                        column: x => x.ForumID,
                        principalTable: "DiscussionForum",
                        principalColumn: "ForumID");
                    table.ForeignKey(
                        name: "FK__Joins__LearnerID__19DFD96B",
                        column: x => x.LearnerID,
                        principalTable: "Learner",
                        principalColumn: "LearnerID");
                });

            migrationBuilder.CreateTable(
                name: "ActivityInstructions",
                columns: table => new
                {
                    ActivityID = table.Column<int>(type: "int", nullable: false),
                    Instruction = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Activity__3B19130FFFF1295E", x => new { x.ActivityID, x.Instruction });
                    table.ForeignKey(
                        name: "FK__ActivityI__Activ__59063A47",
                        column: x => x.ActivityID,
                        principalTable: "LearningActivity",
                        principalColumn: "ActivityID");
                });

            migrationBuilder.CreateTable(
                name: "InteractionLog",
                columns: table => new
                {
                    LogID = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    ActionType = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Duration = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    ActivityID = table.Column<int>(type: "int", nullable: false),
                    LearnerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Interact__5E5499A8BE8A5B86", x => x.LogID);
                    table.ForeignKey(
                        name: "FK__Interacti__Activ__70DDC3D8",
                        column: x => x.ActivityID,
                        principalTable: "LearningActivity",
                        principalColumn: "ActivityID");
                    table.ForeignKey(
                        name: "FK__Interacti__Learn__71D1E811",
                        column: x => x.LearnerID,
                        principalTable: "Learner",
                        principalColumn: "LearnerID");
                });

            migrationBuilder.CreateTable(
                name: "Awards",
                columns: table => new
                {
                    AchievementID = table.Column<int>(type: "int", nullable: false),
                    BadgeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Awards__F6F2B2D7A26A5BB5", x => new { x.AchievementID, x.BadgeID });
                    table.ForeignKey(
                        name: "FK__Awards__Achievem__2B0A656D",
                        column: x => x.AchievementID,
                        principalTable: "Achievement",
                        principalColumn: "AchievementID");
                    table.ForeignKey(
                        name: "FK__Awards__BadgeID__2BFE89A6",
                        column: x => x.BadgeID,
                        principalTable: "Badges",
                        principalColumn: "BadgeID");
                });

            migrationBuilder.CreateTable(
                name: "Earns",
                columns: table => new
                {
                    AchievementID = table.Column<int>(type: "int", nullable: false),
                    LearnerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Earns__91198F2FF40D9D23", x => new { x.AchievementID, x.LearnerID });
                    table.ForeignKey(
                        name: "FK__Earns__Achieveme__2739D489",
                        column: x => x.AchievementID,
                        principalTable: "Achievement",
                        principalColumn: "AchievementID");
                    table.ForeignKey(
                        name: "FK__Earns__LearnerID__282DF8C2",
                        column: x => x.LearnerID,
                        principalTable: "Learner",
                        principalColumn: "LearnerID");
                });

            migrationBuilder.CreateTable(
                name: "Tied",
                columns: table => new
                {
                    EnrollmentID = table.Column<int>(type: "int", nullable: false),
                    LearnerID = table.Column<int>(type: "int", nullable: false),
                    CourseID = table.Column<int>(type: "int", nullable: false),
                    SurveyID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tied__B611B1C446A51BE7", x => new { x.EnrollmentID, x.LearnerID, x.CourseID, x.SurveyID });
                    table.ForeignKey(
                        name: "FK__Tied__3E1D39E1",
                        columns: x => new { x.EnrollmentID, x.LearnerID, x.CourseID },
                        principalTable: "CourseEnrollment",
                        principalColumns: new[] { "EnrollmentID", "LearnerID", "CourseID" });
                    table.ForeignKey(
                        name: "FK__Tied__SurveyID__3F115E1A",
                        column: x => x.SurveyID,
                        principalTable: "Survey",
                        principalColumn: "SurveyID");
                });

            migrationBuilder.CreateTable(
                name: "Expresses",
                columns: table => new
                {
                    LearnerID = table.Column<int>(type: "int", nullable: false),
                    ActivityID = table.Column<int>(type: "int", nullable: false),
                    FeedbackID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Expresse__949EFD6813931B30", x => new { x.LearnerID, x.ActivityID, x.FeedbackID });
                    table.ForeignKey(
                        name: "FK__Expresses__Activ__42E1EEFE",
                        column: x => x.ActivityID,
                        principalTable: "LearningActivity",
                        principalColumn: "ActivityID");
                    table.ForeignKey(
                        name: "FK__Expresses__Feedb__43D61337",
                        column: x => x.FeedbackID,
                        principalTable: "EmotionalFeedback",
                        principalColumn: "FeedbackID");
                    table.ForeignKey(
                        name: "FK__Expresses__Learn__41EDCAC5",
                        column: x => x.LearnerID,
                        principalTable: "Learner",
                        principalColumn: "LearnerID");
                });

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    FeedbackID = table.Column<int>(type: "int", nullable: false),
                    InstructorID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Review__C39BFD41DB9A3D21", x => new { x.FeedbackID, x.InstructorID });
                    table.ForeignKey(
                        name: "FK__Review__Feedback__46B27FE2",
                        column: x => x.FeedbackID,
                        principalTable: "EmotionalFeedback",
                        principalColumn: "FeedbackID");
                    table.ForeignKey(
                        name: "FK__Review__Instruct__47A6A41B",
                        column: x => x.InstructorID,
                        principalTable: "Instructor",
                        principalColumn: "InstructorID");
                });

            migrationBuilder.CreateTable(
                name: "LearningPath",
                columns: table => new
                {
                    PathID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompletionStatus = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    LearningPathDescription = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    AdaptiveRules = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    GoalID = table.Column<int>(type: "int", nullable: false),
                    CreationOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Learning__CD67DC39D4416DA8", x => x.PathID);
                    table.ForeignKey(
                        name: "FK__LearningP__GoalI__45F365D3",
                        column: x => x.GoalID,
                        principalTable: "LearningGoal",
                        principalColumn: "GoalID");
                });

            migrationBuilder.CreateTable(
                name: "Adapt",
                columns: table => new
                {
                    PathID = table.Column<int>(type: "int", nullable: false),
                    InstructorID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Adapt__64B7CC8EF3FCDF0D", x => new { x.PathID, x.InstructorID });
                    table.ForeignKey(
                        name: "FK__Adapt__Instructo__3B40CD36",
                        column: x => x.InstructorID,
                        principalTable: "Instructor",
                        principalColumn: "InstructorID");
                    table.ForeignKey(
                        name: "FK__Adapt__PathID__3A4CA8FD",
                        column: x => x.PathID,
                        principalTable: "LearningPath",
                        principalColumn: "PathID");
                });

            migrationBuilder.CreateTable(
                name: "PersonalizationProfile",
                columns: table => new
                {
                    CreationOrder = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LearnerID = table.Column<int>(type: "int", nullable: false),
                    PersonalityType = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    EmotionalState = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    AccessibilityPreferences = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    PreferredContentTypes = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    NotificationID = table.Column<int>(type: "int", nullable: true),
                    PathID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Personal__9984E276E73EC0E0", x => new { x.CreationOrder, x.LearnerID });
                    table.ForeignKey(
                        name: "FK__Personali__Learn__48CFD27E",
                        column: x => x.LearnerID,
                        principalTable: "Learner",
                        principalColumn: "LearnerID");
                    table.ForeignKey(
                        name: "FK__Personali__Notif__49C3F6B7",
                        column: x => x.NotificationID,
                        principalTable: "Notifications",
                        principalColumn: "NotificationID");
                    table.ForeignKey(
                        name: "FK__Personali__PathI__4AB81AF0",
                        column: x => x.PathID,
                        principalTable: "LearningPath",
                        principalColumn: "PathID");
                });

            migrationBuilder.CreateTable(
                name: "HealthConditions",
                columns: table => new
                {
                    CreationOrder = table.Column<int>(type: "int", nullable: false),
                    LearnerID = table.Column<int>(type: "int", nullable: false),
                    HealthCondition = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__HealthCo__294ED716EDCAF3F6", x => new { x.CreationOrder, x.LearnerID, x.HealthCondition });
                    table.ForeignKey(
                        name: "FK__HealthConditions__4D94879B",
                        columns: x => new { x.CreationOrder, x.LearnerID },
                        principalTable: "PersonalizationProfile",
                        principalColumns: new[] { "CreationOrder", "LearnerID" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Achievement_BadgeID",
                table: "Achievement",
                column: "BadgeID");

            migrationBuilder.CreateIndex(
                name: "IX_Achievement_LearnerID",
                table: "Achievement",
                column: "LearnerID");

            migrationBuilder.CreateIndex(
                name: "IX_Adapt_InstructorID",
                table: "Adapt",
                column: "InstructorID");

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_ModuleID",
                table: "Assessments",
                column: "ModuleID");

            migrationBuilder.CreateIndex(
                name: "IX_Awards_BadgeID",
                table: "Awards",
                column: "BadgeID");

            migrationBuilder.CreateIndex(
                name: "IX_ContentLibrary_CourseID",
                table: "ContentLibrary",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_CourseEnrollment_CourseID",
                table: "CourseEnrollment",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_CourseEnrollment_LearnerID",
                table: "CourseEnrollment",
                column: "LearnerID");

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionForum_ModuleID",
                table: "DiscussionForum",
                column: "ModuleID");

            migrationBuilder.CreateIndex(
                name: "IX_Earns_LearnerID",
                table: "Earns",
                column: "LearnerID");

            migrationBuilder.CreateIndex(
                name: "IX_EmotionalFeedback_LearnerID",
                table: "EmotionalFeedback",
                column: "LearnerID");

            migrationBuilder.CreateIndex(
                name: "IX_Evaluates_AssessmentID",
                table: "Evaluates",
                column: "AssessmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Evaluates_ModuleID",
                table: "Evaluates",
                column: "ModuleID");

            migrationBuilder.CreateIndex(
                name: "IX_Expresses_ActivityID",
                table: "Expresses",
                column: "ActivityID");

            migrationBuilder.CreateIndex(
                name: "IX_Expresses_FeedbackID",
                table: "Expresses",
                column: "FeedbackID");

            migrationBuilder.CreateIndex(
                name: "IX_Grants_RewardID",
                table: "Grants",
                column: "RewardID");

            migrationBuilder.CreateIndex(
                name: "IX_Instructor_UserId",
                table: "Instructor",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InteractionLog_ActivityID",
                table: "InteractionLog",
                column: "ActivityID");

            migrationBuilder.CreateIndex(
                name: "IX_InteractionLog_LearnerID",
                table: "InteractionLog",
                column: "LearnerID");

            migrationBuilder.CreateIndex(
                name: "IX_Joins_ForumID",
                table: "Joins",
                column: "ForumID");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderboardRanks_LearnerID",
                table: "LeaderboardRanks",
                column: "LearnerID");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderboardStudentsCourse_CourseID",
                table: "LeaderboardStudentsCourse",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderboardStudentsCourse_LeaderboardID",
                table: "LeaderboardStudentsCourse",
                column: "LeaderboardID");

            migrationBuilder.CreateIndex(
                name: "IX_Learner_UserId",
                table: "Learner",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LearnersSurvey_SurveyID",
                table: "LearnersSurvey",
                column: "SurveyID");

            migrationBuilder.CreateIndex(
                name: "IX_LearningActivity_ModuleID",
                table: "LearningActivity",
                column: "ModuleID");

            migrationBuilder.CreateIndex(
                name: "IX_LearningGoal_LearnerID",
                table: "LearningGoal",
                column: "LearnerID");

            migrationBuilder.CreateIndex(
                name: "IX_LearningPath_GoalID",
                table: "LearningPath",
                column: "GoalID");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_CourseID",
                table: "Modules",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_Partake_LearnerID",
                table: "Partake",
                column: "LearnerID");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalizationProfile_LearnerID",
                table: "PersonalizationProfile",
                column: "LearnerID");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalizationProfile_NotificationID",
                table: "PersonalizationProfile",
                column: "NotificationID");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalizationProfile_PathID",
                table: "PersonalizationProfile",
                column: "PathID");

            migrationBuilder.CreateIndex(
                name: "IX_Receives_NotificationID",
                table: "Receives",
                column: "NotificationID");

            migrationBuilder.CreateIndex(
                name: "IX_Review_InstructorID",
                table: "Review",
                column: "InstructorID");

            migrationBuilder.CreateIndex(
                name: "IX_SkillProgression_LearnerID",
                table: "SkillProgression",
                column: "LearnerID");

            migrationBuilder.CreateIndex(
                name: "IX_TakenAssessment_LearnerID",
                table: "TakenAssessment",
                column: "LearnerID");

            migrationBuilder.CreateIndex(
                name: "IX_Takes_LearnerID",
                table: "Takes",
                column: "LearnerID");

            migrationBuilder.CreateIndex(
                name: "IX_Teach_CourseID",
                table: "Teach",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_Tied_SurveyID",
                table: "Tied",
                column: "SurveyID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityInstructions");

            migrationBuilder.DropTable(
                name: "Adapt");

            migrationBuilder.DropTable(
                name: "Awards");

            migrationBuilder.DropTable(
                name: "Collaborative_Quests");

            migrationBuilder.DropTable(
                name: "ContentLibrary");

            migrationBuilder.DropTable(
                name: "Earns");

            migrationBuilder.DropTable(
                name: "Evaluates");

            migrationBuilder.DropTable(
                name: "Expertise");

            migrationBuilder.DropTable(
                name: "Expresses");

            migrationBuilder.DropTable(
                name: "Grants");

            migrationBuilder.DropTable(
                name: "HealthConditions");

            migrationBuilder.DropTable(
                name: "InteractionLog");

            migrationBuilder.DropTable(
                name: "Joins");

            migrationBuilder.DropTable(
                name: "LeaderboardRanks");

            migrationBuilder.DropTable(
                name: "LeaderboardStudentsCourse");

            migrationBuilder.DropTable(
                name: "LearnerSkills");

            migrationBuilder.DropTable(
                name: "LearnersSurvey");

            migrationBuilder.DropTable(
                name: "LearningPreferences");

            migrationBuilder.DropTable(
                name: "Partake");

            migrationBuilder.DropTable(
                name: "QuestReward");

            migrationBuilder.DropTable(
                name: "Receives");

            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.DropTable(
                name: "Skill_Mastery_Quests");

            migrationBuilder.DropTable(
                name: "SkillProgression");

            migrationBuilder.DropTable(
                name: "SurveyQuestions");

            migrationBuilder.DropTable(
                name: "TakenAssessment");

            migrationBuilder.DropTable(
                name: "Takes");

            migrationBuilder.DropTable(
                name: "TargetTraits");

            migrationBuilder.DropTable(
                name: "Teach");

            migrationBuilder.DropTable(
                name: "Tied");

            migrationBuilder.DropTable(
                name: "Achievement");

            migrationBuilder.DropTable(
                name: "Reward");

            migrationBuilder.DropTable(
                name: "PersonalizationProfile");

            migrationBuilder.DropTable(
                name: "LearningActivity");

            migrationBuilder.DropTable(
                name: "DiscussionForum");

            migrationBuilder.DropTable(
                name: "Leaderboard");

            migrationBuilder.DropTable(
                name: "EmotionalFeedback");

            migrationBuilder.DropTable(
                name: "Quests");

            migrationBuilder.DropTable(
                name: "Assessments");

            migrationBuilder.DropTable(
                name: "Instructor");

            migrationBuilder.DropTable(
                name: "CourseEnrollment");

            migrationBuilder.DropTable(
                name: "Survey");

            migrationBuilder.DropTable(
                name: "Badges");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "LearningPath");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropTable(
                name: "LearningGoal");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Learner");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
