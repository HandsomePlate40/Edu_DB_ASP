using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Edu_DB_ASP.Models;

public partial class EduDbContext : DbContext
{
    public EduDbContext()
    {
    }

    public EduDbContext(DbContextOptions<EduDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Achievement> Achievements { get; set; }

    public virtual DbSet<ActivityInstruction> ActivityInstructions { get; set; }

    public virtual DbSet<Assessment> Assessments { get; set; }

    public virtual DbSet<Badge> Badges { get; set; }

    public virtual DbSet<CollaborativeQuest> CollaborativeQuests { get; set; }

    public virtual DbSet<ContentLibrary> ContentLibraries { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseEnrollment> CourseEnrollments { get; set; }

    public virtual DbSet<DiscussionForum> DiscussionForums { get; set; }

    public virtual DbSet<EmotionalFeedback> EmotionalFeedbacks { get; set; }

    public virtual DbSet<Evaluate> Evaluates { get; set; }

    public virtual DbSet<Expertise> Expertises { get; set; }

    public virtual DbSet<Express> Expresses { get; set; }

    public virtual DbSet<HealthCondition> HealthConditions { get; set; }

    public virtual DbSet<Instructor> Instructors { get; set; }

    public virtual DbSet<InteractionLog> InteractionLogs { get; set; }

    public virtual DbSet<Join> Joins { get; set; }

    public virtual DbSet<Leaderboard> Leaderboards { get; set; }

    public virtual DbSet<LeaderboardRank> LeaderboardRanks { get; set; }

    public virtual DbSet<LeaderboardStudentsCourse> LeaderboardStudentsCourses { get; set; }

    public virtual DbSet<Learner> Learners { get; set; }

    public virtual DbSet<LearnerSkill> LearnerSkills { get; set; }

    public virtual DbSet<LearningActivity> LearningActivities { get; set; }

    public virtual DbSet<LearningGoal> LearningGoals { get; set; }

    public virtual DbSet<LearningPath> LearningPaths { get; set; }

    public virtual DbSet<LearningPreference> LearningPreferences { get; set; }

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<PersonalizationProfile> PersonalizationProfiles { get; set; }

    public virtual DbSet<Quest> Quests { get; set; }

    public virtual DbSet<QuestReward> QuestRewards { get; set; }

    public virtual DbSet<Reward> Rewards { get; set; }

    public virtual DbSet<SkillMasteryQuest> SkillMasteryQuests { get; set; }

    public virtual DbSet<SkillProgression> SkillProgressions { get; set; }

    public virtual DbSet<Survey> Surveys { get; set; }

    public virtual DbSet<SurveyQuestion> SurveyQuestions { get; set; }

    public virtual DbSet<TakenAssessment> TakenAssessments { get; set; }

    public virtual DbSet<TargetTrait> TargetTraits { get; set; }

    public virtual DbSet<Admin> Admins { get; set; } 

    

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=edu_DB;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Achievement>(entity =>
        {
            entity.HasKey(e => e.AchievementId).HasName("PK__Achievem__276330E0200B5C44");

            entity.ToTable("Achievement");

            entity.HasIndex(e => e.BadgeId, "IX_Achievement_BadgeID");

            entity.HasIndex(e => e.LearnerId, "IX_Achievement_LearnerID");

            entity.Property(e => e.AchievementId).HasColumnName("AchievementID");
            entity.Property(e => e.AchievementDescription).IsUnicode(false);
            entity.Property(e => e.AchievementType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.BadgeId).HasColumnName("BadgeID");
            entity.Property(e => e.LearnerId).HasColumnName("LearnerID");

            entity.HasOne(d => d.Badge).WithMany(p => p.Achievements)
                .HasForeignKey(d => d.BadgeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Achieveme__Badge__628FA481");

            entity.HasOne(d => d.Learner).WithMany(p => p.Achievements)
                .HasForeignKey(d => d.LearnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Achieveme__Learn__619B8048");

            entity.HasMany(d => d.Badges).WithMany(p => p.AchievementsNavigation)
                .UsingEntity<Dictionary<string, object>>(
                    "Award",
                    r => r.HasOne<Badge>().WithMany()
                        .HasForeignKey("BadgeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Awards__BadgeID__2BFE89A6"),
                    l => l.HasOne<Achievement>().WithMany()
                        .HasForeignKey("AchievementId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Awards__Achievem__2B0A656D"),
                    j =>
                    {
                        j.HasKey("AchievementId", "BadgeId").HasName("PK__Awards__F6F2B2D7A26A5BB5");
                        j.ToTable("Awards");
                        j.HasIndex(new[] { "BadgeId" }, "IX_Awards_BadgeID");
                        j.IndexerProperty<int>("AchievementId").HasColumnName("AchievementID");
                        j.IndexerProperty<int>("BadgeId").HasColumnName("BadgeID");
                    });

            entity.HasMany(d => d.Learners).WithMany(p => p.AchievementsNavigation)
                .UsingEntity<Dictionary<string, object>>(
                    "Earn",
                    r => r.HasOne<Learner>().WithMany()
                        .HasForeignKey("LearnerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Earns__LearnerID__282DF8C2"),
                    l => l.HasOne<Achievement>().WithMany()
                        .HasForeignKey("AchievementId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Earns__Achieveme__2739D489"),
                    j =>
                    {
                        j.HasKey("AchievementId", "LearnerId").HasName("PK__Earns__91198F2FF40D9D23");
                        j.ToTable("Earns");
                        j.HasIndex(new[] { "LearnerId" }, "IX_Earns_LearnerID");
                        j.IndexerProperty<int>("AchievementId").HasColumnName("AchievementID");
                        j.IndexerProperty<int>("LearnerId").HasColumnName("LearnerID");
                    });
        });

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admin__719FE4E8");

            entity.ToTable("Admins");

            entity.Property(e => e.AdminId).HasColumnName("AdminID");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasIndex(e => e.Email).IsUnique(); 

            entity.Property(e => e.FirstName)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.ProfilePictureUrl)
                .HasMaxLength(500)
                .IsUnicode(false);
        });


        modelBuilder.Entity<ActivityInstruction>(entity =>
        {
            entity.HasKey(e => new { e.ActivityId, e.Instruction }).HasName("PK__Activity__3B19130FFFF1295E");

            entity.Property(e => e.ActivityId).HasColumnName("ActivityID");
            entity.Property(e => e.Instruction)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Activity).WithMany(p => p.ActivityInstructions)
                .HasForeignKey(d => d.ActivityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ActivityI__Activ__59063A47");
        });

        modelBuilder.Entity<Assessment>(entity =>
        {
            entity.HasKey(e => e.AssessmentId).HasName("PK__Assessme__3D2BF83E5A3CB530");

            entity.HasIndex(e => e.ModuleId, "IX_Assessments_ModuleID");

            entity.Property(e => e.AssessmentId)
                .ValueGeneratedNever()
                .HasColumnName("AssessmentID");
            entity.Property(e => e.AssessmentDescription)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.GradingCriteria)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.MaxScore).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ModuleId).HasColumnName("ModuleID");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Weightage).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Module).WithMany(p => p.Assessments)
                .HasForeignKey(d => d.ModuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Assessmen__Modul__07C12930");
        });

        modelBuilder.Entity<Badge>(entity =>
        {
            entity.HasKey(e => e.BadgeId).HasName("PK__Badges__1918237CBBA0B7BA");

            entity.Property(e => e.BadgeId).HasColumnName("BadgeID");
            entity.Property(e => e.BadgeDescription)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.BadgeTitle)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CriteriaToUnlock)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.PointsValue).HasColumnType("decimal(5, 2)");

            entity.HasMany(d => d.Learners).WithMany(p => p.Badges)
                .UsingEntity<Dictionary<string, object>>(
                    "Take",
                    r => r.HasOne<Learner>().WithMany()
                        .HasForeignKey("LearnerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Takes__LearnerID__37703C52"),
                    l => l.HasOne<Badge>().WithMany()
                        .HasForeignKey("BadgeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Takes__BadgeID__367C1819"),
                    j =>
                    {
                        j.HasKey("BadgeId", "LearnerId").HasName("PK__Takes__AF629CB3EB07A3C2");
                        j.ToTable("Takes");
                        j.HasIndex(new[] { "LearnerId" }, "IX_Takes_LearnerID");
                        j.IndexerProperty<int>("BadgeId").HasColumnName("BadgeID");
                        j.IndexerProperty<int>("LearnerId").HasColumnName("LearnerID");
                    });
        });

        modelBuilder.Entity<CollaborativeQuest>(entity =>
        {
            entity.HasKey(e => e.QuestId).HasName("PK__Collabor__B6619ACBF03D3510");

            entity.ToTable("Collaborative_Quests");

            entity.Property(e => e.QuestId)
                .ValueGeneratedNever()
                .HasColumnName("QuestID");
            entity.Property(e => e.DeadLine).HasColumnType("datetime");
            entity.Property(e => e.MaxParticipants).HasColumnName("Max_Participants");

            entity.HasOne(d => d.Quest).WithOne(p => p.CollaborativeQuest)
                .HasForeignKey<CollaborativeQuest>(d => d.QuestId)
                .HasConstraintName("FK__Collabora__Quest__7D439ABD");
        });

        modelBuilder.Entity<ContentLibrary>(entity =>
        {
            entity.HasKey(e => e.ContentId).HasName("PK__ContentL__2907A87EC282FE42");

            entity.ToTable("ContentLibrary");

            entity.HasIndex(e => e.CourseId, "IX_ContentLibrary_CourseID");

            entity.Property(e => e.ContentId).HasColumnName("ContentID");
            entity.Property(e => e.ContentType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ContentUrl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("ContentURL");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.DifficultyLevel)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Keywords)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.LearningObjectives)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Course).WithMany(p => p.ContentLibraries)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__ContentLi__Cours__1332DBDC");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Courses__C92D7187B5609625");

            entity.Property(e => e.CourseId)
                .ValueGeneratedNever()
                .HasColumnName("CourseID");
            entity.Property(e => e.CourseDescription)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CreditPoints).HasColumnType("decimal(4, 2)");
            entity.Property(e => e.DifficultyLevel)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LearningObjectives).HasColumnType("text");
            entity.Property(e => e.Prerequisites)
                .HasMaxLength(212)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CourseEnrollment>(entity =>
        {
            entity.HasKey(e => new { e.EnrollmentId, e.LearnerId, e.CourseId }).HasName("PK__CourseEn__4FDBE545C4B4E420");

            entity.ToTable("CourseEnrollment");

            entity.HasIndex(e => e.CourseId, "IX_CourseEnrollment_CourseID");

            entity.HasIndex(e => e.LearnerId, "IX_CourseEnrollment_LearnerID");

            entity.Property(e => e.EnrollmentId).HasColumnName("EnrollmentID");
            entity.Property(e => e.LearnerId).HasColumnName("LearnerID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.EnrollmentStatus)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Course).WithMany(p => p.CourseEnrollments)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__CourseEnr__Cours__5CD6CB2B");

            entity.HasOne(d => d.Learner).WithMany(p => p.CourseEnrollments)
                .HasForeignKey(d => d.LearnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CourseEnr__Learn__5BE2A6F2");

            entity.HasMany(d => d.Surveys).WithMany(p => p.CourseEnrollments)
                .UsingEntity<Dictionary<string, object>>(
                    "Tied",
                    r => r.HasOne<Survey>().WithMany()
                        .HasForeignKey("SurveyId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Tied__SurveyID__3F115E1A"),
                    l => l.HasOne<CourseEnrollment>().WithMany()
                        .HasForeignKey("EnrollmentId", "LearnerId", "CourseId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Tied__3E1D39E1"),
                    j =>
                    {
                        j.HasKey("EnrollmentId", "LearnerId", "CourseId", "SurveyId").HasName("PK__Tied__B611B1C446A51BE7");
                        j.ToTable("Tied");
                        j.HasIndex(new[] { "SurveyId" }, "IX_Tied_SurveyID");
                        j.IndexerProperty<int>("EnrollmentId").HasColumnName("EnrollmentID");
                        j.IndexerProperty<int>("LearnerId").HasColumnName("LearnerID");
                        j.IndexerProperty<int>("CourseId").HasColumnName("CourseID");
                        j.IndexerProperty<int>("SurveyId").HasColumnName("SurveyID");
                    });
        });

        modelBuilder.Entity<DiscussionForum>(entity =>
        {
            entity.HasKey(e => e.ForumId).HasName("PK__Discussi__E210AC4F7B9376C5");

            entity.ToTable("DiscussionForum");

            entity.HasIndex(e => e.ModuleId, "IX_DiscussionForum_ModuleID");

            entity.Property(e => e.ForumId)
                .ValueGeneratedNever()
                .HasColumnName("ForumID");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.LastActiveTimestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModuleId).HasColumnName("ModuleID");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Module).WithMany(p => p.DiscussionForums)
                .HasForeignKey(d => d.ModuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Discussio__Modul__17036CC0");
        });

        modelBuilder.Entity<EmotionalFeedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Emotiona__6A4BEDF6C079427F");

            entity.ToTable("EmotionalFeedback");

            entity.HasIndex(e => e.LearnerId, "IX_EmotionalFeedback_LearnerID");

            entity.Property(e => e.FeedbackId).HasColumnName("FeedbackID");
            entity.Property(e => e.EmotionalState)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LearnerId).HasColumnName("LearnerID");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Learner).WithMany(p => p.EmotionalFeedbacks)
                .HasForeignKey(d => d.LearnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Emotional__Learn__75A278F5");

            entity.HasMany(d => d.Instructors).WithMany(p => p.Feedbacks)
                .UsingEntity<Dictionary<string, object>>(
                    "Review",
                    r => r.HasOne<Instructor>().WithMany()
                        .HasForeignKey("InstructorId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Review_Instructor"),
                    l => l.HasOne<EmotionalFeedback>().WithMany()
                        .HasForeignKey("FeedbackId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Review__Feedback__46B27FE2"),
                    j =>
                    {
                        j.HasKey("FeedbackId", "InstructorId").HasName("PK__Review__C39BFD41DB9A3D21");
                        j.ToTable("Review");
                        j.HasIndex(new[] { "InstructorId" }, "IX_Review_InstructorID");
                        j.IndexerProperty<int>("FeedbackId").HasColumnName("FeedbackID");
                        j.IndexerProperty<int>("InstructorId").HasColumnName("InstructorID");
                    });
        });

        modelBuilder.Entity<Evaluate>(entity =>
        {
            entity.HasKey(e => new { e.CourseId, e.ModuleId, e.AssessmentId }).HasName("PK__Evaluate__94A71D07350C7833");

            entity.HasIndex(e => e.AssessmentId, "IX_Evaluates_AssessmentID");

            entity.HasIndex(e => e.ModuleId, "IX_Evaluates_ModuleID");

            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.ModuleId).HasColumnName("ModuleID");
            entity.Property(e => e.AssessmentId).HasColumnName("AssessmentID");

            entity.HasOne(d => d.Assessment).WithMany(p => p.Evaluates)
                .HasForeignKey(d => d.AssessmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Evaluates__Asses__503BEA1C");

            entity.HasOne(d => d.Course).WithMany(p => p.Evaluates)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Evaluates__Cours__4E53A1AA");

            entity.HasOne(d => d.Module).WithMany(p => p.Evaluates)
                .HasForeignKey(d => d.ModuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Evaluates__Modul__4F47C5E3");
        });

        modelBuilder.Entity<Expertise>(entity =>
        {
            entity.HasKey(e => new { e.InstructorId, e.ExpertiseArea }).HasName("PK__Expertis__2B5DADA429D76C0C");

            entity.ToTable("Expertise");

            entity.Property(e => e.InstructorId).HasColumnName("InstructorID");
            entity.Property(e => e.ExpertiseArea)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Instructor).WithMany(p => p.Expertises)
                .HasForeignKey(d => d.InstructorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Expertise_Instructor");
        });

        modelBuilder.Entity<Express>(entity =>
        {
            entity.HasKey(e => new { e.LearnerId, e.ActivityId, e.FeedbackId }).HasName("PK__Expresse__949EFD6813931B30");

            entity.HasIndex(e => e.ActivityId, "IX_Expresses_ActivityID");

            entity.HasIndex(e => e.FeedbackId, "IX_Expresses_FeedbackID");

            entity.Property(e => e.LearnerId).HasColumnName("LearnerID");
            entity.Property(e => e.ActivityId).HasColumnName("ActivityID");
            entity.Property(e => e.FeedbackId).HasColumnName("FeedbackID");

            entity.HasOne(d => d.Activity).WithMany(p => p.Expresses)
                .HasForeignKey(d => d.ActivityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Expresses__Activ__42E1EEFE");

            entity.HasOne(d => d.Feedback).WithMany(p => p.Expresses)
                .HasForeignKey(d => d.FeedbackId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Expresses__Feedb__43D61337");

            entity.HasOne(d => d.Learner).WithMany(p => p.Expresses)
                .HasForeignKey(d => d.LearnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Expresses__Learn__41EDCAC5");
        });

        modelBuilder.Entity<HealthCondition>(entity =>
        {
            entity.HasKey(e => new { e.CreationOrder, e.LearnerId, e.HealthCondition1 }).HasName("PK__HealthCo__294ED716EDCAF3F6");

            entity.Property(e => e.LearnerId).HasColumnName("LearnerID");
            entity.Property(e => e.HealthCondition1)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("HealthCondition");

            entity.HasOne(d => d.PersonalizationProfile).WithMany(p => p.HealthConditions)
                .HasForeignKey(d => new { d.CreationOrder, d.LearnerId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HealthConditions__4D94879B");
        });
        modelBuilder.Entity<Instructor>(entity =>
        {
            entity.HasKey(e => e.InstructorId).HasName("PK__Instruct__9D010B7B01C75B11");

            entity.ToTable("Instructor");

            entity.Property(e => e.InstructorId).HasColumnName("InstructorID");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasIndex(e => e.Email).IsUnique();

            entity.Property(e => e.InstructorName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Qualifications)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ProfilePictureUrl)
                .HasMaxLength(500)
                .IsUnicode(false); // Add this line

            entity.HasMany(d => d.Courses).WithMany(p => p.Instructors)
                .UsingEntity<Dictionary<string, object>>(
                    "Teach",
                    r => r.HasOne<Course>().WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Teach__CourseID__540C7B00"),
                    l => l.HasOne<Instructor>().WithMany()
                        .HasForeignKey("InstructorId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Teach_Instructor"),
                    j =>
                    {
                        j.HasKey("InstructorId", "CourseId").HasName("PK__Teach__F193DC63B5052AB2");
                        j.ToTable("Teach");
                        j.HasIndex(new[] { "CourseId" }, "IX_Teach_CourseID");
                        j.IndexerProperty<int>("InstructorId").HasColumnName("InstructorID");
                        j.IndexerProperty<int>("CourseId").HasColumnName("CourseID");
                    });
        });



        modelBuilder.Entity<InteractionLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__Interact__5E5499A8BE8A5B86");

            entity.ToTable("InteractionLog");

            entity.HasIndex(e => e.ActivityId, "IX_InteractionLog_ActivityID");

            entity.HasIndex(e => e.LearnerId, "IX_InteractionLog_LearnerID");

            entity.Property(e => e.LogId)
                .ValueGeneratedNever()
                .HasColumnName("LogID");
            entity.Property(e => e.ActionType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ActivityId).HasColumnName("ActivityID");
            entity.Property(e => e.Duration).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.LearnerId).HasColumnName("LearnerID");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Activity).WithMany(p => p.InteractionLogs)
                .HasForeignKey(d => d.ActivityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Interacti__Activ__70DDC3D8");

            entity.HasOne(d => d.Learner).WithMany(p => p.InteractionLogs)
                .HasForeignKey(d => d.LearnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Interacti__Learn__71D1E811");
        });

        modelBuilder.Entity<Join>(entity =>
        {
            entity.HasKey(e => new { e.LearnerId, e.ForumId }).HasName("PK__Joins__898AF63EBA3E703D");

            entity.HasIndex(e => e.ForumId, "IX_Joins_ForumID");

            entity.Property(e => e.LearnerId).HasColumnName("LearnerID");
            entity.Property(e => e.ForumId).HasColumnName("ForumID");
            entity.Property(e => e.Post).HasColumnType("text");

            entity.HasOne(d => d.Forum).WithMany(p => p.Joins)
                .HasForeignKey(d => d.ForumId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Joins__ForumID__1AD3FDA4");

            entity.HasOne(d => d.Learner).WithMany(p => p.Joins)
                .HasForeignKey(d => d.LearnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Joins__LearnerID__19DFD96B");
        });

        modelBuilder.Entity<Leaderboard>(entity =>
        {
            entity.HasKey(e => e.LeaderboardId).HasName("PK__Leaderbo__B358A1E686B80F17");

            entity.ToTable("Leaderboard");

            entity.Property(e => e.LeaderboardId)
                .ValueGeneratedNever()
                .HasColumnName("LeaderboardID");
            entity.Property(e => e.Season)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TotalPoints).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<LeaderboardRank>(entity =>
        {
            entity.HasKey(e => new { e.LeaderboardId, e.LearnerId }).HasName("PK__Leaderbo__05221E2914A09FB2");

            entity.HasIndex(e => e.LearnerId, "IX_LeaderboardRanks_LearnerID");

            entity.Property(e => e.LeaderboardId).HasColumnName("LeaderboardID");
            entity.Property(e => e.LearnerId).HasColumnName("LearnerID");
            entity.Property(e => e.LeaderboardRank1).HasColumnName("LeaderboardRank");

            entity.HasOne(d => d.Leaderboard).WithMany(p => p.LeaderboardRanks)
                .HasForeignKey(d => d.LeaderboardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Leaderboa__Leade__6754599E");

            entity.HasOne(d => d.Learner).WithMany(p => p.LeaderboardRanks)
                .HasForeignKey(d => d.LearnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Leaderboa__Learn__68487DD7");
        });

        modelBuilder.Entity<LeaderboardStudentsCourse>(entity =>
        {
            entity.HasKey(e => new { e.LearnerId, e.LeaderboardId }).HasName("PK__Leaderbo__1C9E76E421B43CAB");

            entity.ToTable("LeaderboardStudentsCourse");

            entity.HasIndex(e => e.CourseId, "IX_LeaderboardStudentsCourse_CourseID");

            entity.HasIndex(e => e.LeaderboardId, "IX_LeaderboardStudentsCourse_LeaderboardID");

            entity.Property(e => e.LearnerId).HasColumnName("LearnerID");
            entity.Property(e => e.LeaderboardId).HasColumnName("LeaderboardID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");

            entity.HasOne(d => d.Course).WithMany(p => p.LeaderboardStudentsCourses)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Leaderboa__Cours__6B24EA82");

            entity.HasOne(d => d.Leaderboard).WithMany(p => p.LeaderboardStudentsCourses)
                .HasForeignKey(d => d.LeaderboardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Leaderboa__Leade__6D0D32F4");

            entity.HasOne(d => d.Learner).WithMany(p => p.LeaderboardStudentsCourses)
                .HasForeignKey(d => d.LearnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Leaderboa__Learn__6C190EBB");
        });

        modelBuilder.Entity<Learner>(entity =>
        {
            entity.HasKey(e => e.LearnerId).HasName("PK__Learner__67ABFCFA1EEFDCD8");

            entity.ToTable("Learner");

            entity.Property(e => e.LearnerId).HasColumnName("LearnerID");
            entity.Property(e => e.CountryOfOrigin)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CulturalBackground)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("cultural_background");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.EmotionalProfile)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ExperienceLevel)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.Gender)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("gender");
            entity.Property(e => e.LastName)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.MentalHealth)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.PersonalityTraits)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PhysicalHealth)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasIndex(e => e.Email).IsUnique(); 

            entity.HasMany(d => d.Notifications).WithMany(p => p.Learners)
                .UsingEntity<Dictionary<string, object>>(
                    "Receife",
                    r => r.HasOne<Notification>().WithMany()
                        .HasForeignKey("NotificationId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Receives__Notifi__4B7734FF"),
                    l => l.HasOne<Learner>().WithMany()
                        .HasForeignKey("LearnerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Receives__Learne__4A8310C6"),
                    j =>
                    {
                        j.HasKey("LearnerId", "NotificationId").HasName("PK__Receives__55A70E193D54808C");
                        j.ToTable("Receives");
                        j.HasIndex(new[] { "NotificationId" }, "IX_Receives_NotificationID");
                        j.IndexerProperty<int>("LearnerId").HasColumnName("LearnerID");
                        j.IndexerProperty<int>("NotificationId").HasColumnName("NotificationID");
                    });

            entity.HasMany(d => d.Surveys).WithMany(p => p.Learners)
                .UsingEntity<Dictionary<string, object>>(
                    "LearnersSurvey",
                    r => r.HasOne<Survey>().WithMany()
                        .HasForeignKey("SurveyId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__LearnersS__Surve__245D67DE"),
                    l => l.HasOne<Learner>().WithMany()
                        .HasForeignKey("LearnerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__LearnersS__Learn__236943A5"),
                    j =>
                    {
                        j.HasKey("LearnerId", "SurveyId").HasName("PK__Learners__ADFF7D039B217D05");
                        j.ToTable("LearnersSurvey");
                        j.HasIndex(new[] { "SurveyId" }, "IX_LearnersSurvey_SurveyID");
                        j.IndexerProperty<int>("LearnerId").HasColumnName("LearnerID");
                        j.IndexerProperty<int>("SurveyId").HasColumnName("SurveyID");
                    });
        });

        modelBuilder.Entity<LearnerSkill>(entity =>
        {
            entity.HasKey(e => new { e.LearnerId, e.Skill }).HasName("PK__LearnerS__C45BDEA5E48E410D");

            entity.Property(e => e.LearnerId).HasColumnName("LearnerID");
            entity.Property(e => e.Skill)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("skill");

            entity.HasOne(d => d.Learner).WithMany(p => p.LearnerSkills)
                .HasForeignKey(d => d.LearnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LearnerSk__Learn__3A81B327");
        });

        modelBuilder.Entity<LearningActivity>(entity =>
        {
            entity.HasKey(e => e.ActivityId).HasName("PK__Learning__45F4A7F148FC20D3");

            entity.ToTable("LearningActivity");

            entity.HasIndex(e => e.ModuleId, "IX_LearningActivity_ModuleID");

            entity.Property(e => e.ActivityId).HasColumnName("ActivityID");
            entity.Property(e => e.ActivityType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.MaxPoints).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.ModuleId).HasColumnName("ModuleID");

            entity.HasOne(d => d.Module).WithMany(p => p.LearningActivities)
                .HasForeignKey(d => d.ModuleId)
                .HasConstraintName("FK__LearningA__Modul__5629CD9C");
        });

        modelBuilder.Entity<LearningGoal>(entity =>
        {
            entity.HasKey(e => e.GoalId).HasName("PK__Learning__8A4FFF311BEAC4EC");

            entity.ToTable("LearningGoal");

            entity.HasIndex(e => e.LearnerId, "IX_LearningGoal_LearnerID");

            entity.Property(e => e.GoalId)
                .ValueGeneratedNever()
                .HasColumnName("GoalID");
            entity.Property(e => e.GoalDescription)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.LearnerId).HasColumnName("LearnerID");
            entity.Property(e => e.ObjectiveType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ProgressStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TimeBound).HasColumnType("datetime");

            entity.HasOne(d => d.Learner).WithMany(p => p.LearningGoals)
                .HasForeignKey(d => d.LearnerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__LearningG__Learn__4316F928");
        });

        modelBuilder.Entity<LearningPath>(entity =>
        {
            entity.HasKey(e => e.PathId).HasName("PK__Learning__CD67DC39D4416DA8");

            entity.ToTable("LearningPath");

            entity.HasIndex(e => e.GoalId, "IX_LearningPath_GoalID");

            entity.Property(e => e.PathId).HasColumnName("PathID");
            entity.Property(e => e.AdaptiveRules)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CompletionStatus)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.GoalId).HasColumnName("GoalID");
            entity.Property(e => e.LearningPathDescription)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.Goal).WithMany(p => p.LearningPaths)
                .HasForeignKey(d => d.GoalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LearningP__GoalI__45F365D3");

            entity.HasMany(d => d.Instructors).WithMany(p => p.Paths)
                .UsingEntity<Dictionary<string, object>>(
                    "Adapt",
                    r => r.HasOne<Instructor>().WithMany()
                        .HasForeignKey("InstructorId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Adapt_Instructor"),
                    l => l.HasOne<LearningPath>().WithMany()
                        .HasForeignKey("PathId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Adapt__PathID__3A4CA8FD"),
                    j =>
                    {
                        j.HasKey("PathId", "InstructorId").HasName("PK__Adapt__64B7CC8EF3FCDF0D");
                        j.ToTable("Adapt");
                        j.HasIndex(new[] { "InstructorId" }, "IX_Adapt_InstructorID");
                        j.IndexerProperty<int>("PathId").HasColumnName("PathID");
                        j.IndexerProperty<int>("InstructorId").HasColumnName("InstructorID");
                    });
        });

        modelBuilder.Entity<LearningPreference>(entity =>
        {
            entity.HasKey(e => new { e.LearnerId, e.Preference }).HasName("PK__Learning__6032E158494B9BDA");

            entity.Property(e => e.LearnerId).HasColumnName("LearnerID");
            entity.Property(e => e.Preference)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("preference");

            entity.HasOne(d => d.Learner).WithMany(p => p.LearningPreferences)
                .HasForeignKey(d => d.LearnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LearningP__Learn__3D5E1FD2");
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasKey(e => e.ModuleId).HasName("PK__Modules__2B7477876BE6F7D6");

            entity.HasIndex(e => e.CourseId, "IX_Modules_CourseID");

            entity.Property(e => e.ModuleId).HasColumnName("ModuleID");
            entity.Property(e => e.ContentType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ContentUrl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("ContentURL");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.ModuleDifficulty)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Course).WithMany(p => p.Modules)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__Modules__CourseI__5070F446");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E321C986044");

            entity.Property(e => e.NotificationId).HasColumnName("NotificationID");
            entity.Property(e => e.MessageBody)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UrgencyLevel)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PersonalizationProfile>(entity =>
        {
            entity.HasKey(e => new { e.CreationOrder, e.LearnerId }).HasName("PK__Personal__9984E276E73EC0E0");

            entity.ToTable("PersonalizationProfile");

            entity.HasIndex(e => e.LearnerId, "IX_PersonalizationProfile_LearnerID");

            entity.HasIndex(e => e.NotificationId, "IX_PersonalizationProfile_NotificationID");

            entity.HasIndex(e => e.PathId, "IX_PersonalizationProfile_PathID");

            entity.Property(e => e.CreationOrder).ValueGeneratedOnAdd();
            entity.Property(e => e.LearnerId).HasColumnName("LearnerID");
            entity.Property(e => e.AccessibilityPreferences)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.EmotionalState)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.NotificationId).HasColumnName("NotificationID");
            entity.Property(e => e.PathId).HasColumnName("PathID");
            entity.Property(e => e.PersonalityType)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PreferredContentTypes)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Learner)
                .WithMany(p => p.PersonalizationProfiles)
                .HasForeignKey(d => d.LearnerId)
                .OnDelete(DeleteBehavior.Cascade)
                
                .HasConstraintName("FK__Personali__Learn__48CFD27E");

            entity.HasOne(d => d.Notification)
                .WithMany(p => p.PersonalizationProfiles)
                .HasForeignKey(d => d.NotificationId)
                .HasConstraintName("FK__Personali__Notif__49C3F6B7");

            entity.HasOne(d => d.Path)
                .WithMany(p => p.PersonalizationProfiles)
                .HasForeignKey(d => d.PathId)
                .HasConstraintName("FK__Personali__PathI__4AB81AF0");
        });

        modelBuilder.Entity<Quest>(entity =>
        {
            entity.HasKey(e => e.QuestId).HasName("PK__Quests__B6619ACB885D6696");

            entity.Property(e => e.QuestId).HasColumnName("QuestID");
            entity.Property(e => e.Criteria)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.DifficultyLevel)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.QuestDescription)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.QuestType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasMany(d => d.Learners).WithMany(p => p.Quests)
                .UsingEntity<Dictionary<string, object>>(
                    "Partake",
                    r => r.HasOne<Learner>().WithMany()
                        .HasForeignKey("LearnerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Partake__Learner__339FAB6E"),
                    l => l.HasOne<Quest>().WithMany()
                        .HasForeignKey("QuestId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Partake__QuestID__32AB8735"),
                    j =>
                    {
                        j.HasKey("QuestId", "LearnerId").HasName("PK__Partake__001B2504CA672BC5");
                        j.ToTable("Partake");
                        j.HasIndex(new[] { "LearnerId" }, "IX_Partake_LearnerID");
                        j.IndexerProperty<int>("QuestId").HasColumnName("QuestID");
                        j.IndexerProperty<int>("LearnerId").HasColumnName("LearnerID");
                    });

            entity.HasMany(d => d.Rewards).WithMany(p => p.Quests)
                .UsingEntity<Dictionary<string, object>>(
                    "Grant",
                    r => r.HasOne<Reward>().WithMany()
                        .HasForeignKey("RewardId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Grants__RewardID__2FCF1A8A"),
                    l => l.HasOne<Quest>().WithMany()
                        .HasForeignKey("QuestId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Grants__QuestID__2EDAF651"),
                    j =>
                    {
                        j.HasKey("QuestId", "RewardId").HasName("PK__Grants__3E449B92A94CEF25");
                        j.ToTable("Grants");
                        j.HasIndex(new[] { "RewardId" }, "IX_Grants_RewardID");
                        j.IndexerProperty<int>("QuestId").HasColumnName("QuestID");
                        j.IndexerProperty<int>("RewardId").HasColumnName("RewardID");
                    });
        });

        modelBuilder.Entity<QuestReward>(entity =>
        {
            entity.HasKey(e => new { e.QuestId, e.Rewards }).HasName("PK__QuestRew__D749EC5578916AF7");

            entity.ToTable("QuestReward");

            entity.Property(e => e.QuestId).HasColumnName("QuestID");
            entity.Property(e => e.Rewards)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Quest).WithMany(p => p.QuestRewards)
                .HasForeignKey(d => d.QuestId)
                .HasConstraintName("FK__QuestRewa__Quest__00200768");
        });

        modelBuilder.Entity<Reward>(entity =>
        {
            entity.HasKey(e => e.RewardId).HasName("PK__Reward__82501599E3A34849");

            entity.ToTable("Reward");

            entity.Property(e => e.RewardId)
                .ValueGeneratedNever()
                .HasColumnName("RewardID");
            entity.Property(e => e.RewardDescription)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.RewardType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RewardValue).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<SkillMasteryQuest>(entity =>
        {
            entity.HasKey(e => e.QuestId).HasName("PK__Skill_Ma__B6619ACBBB199C12");

            entity.ToTable("Skill_Mastery_Quests");

            entity.Property(e => e.QuestId)
                .ValueGeneratedNever()
                .HasColumnName("QuestID");
            entity.Property(e => e.Skills)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Quest).WithOne(p => p.SkillMasteryQuest)
                .HasForeignKey<SkillMasteryQuest>(d => d.QuestId)
                .HasConstraintName("FK__Skill_Mas__Quest__7A672E12");
        });

        modelBuilder.Entity<SkillProgression>(entity =>
        {
            entity.HasKey(e => e.ProgressId).HasName("PK__SkillPro__BAE29C85DEE3B5F7");

            entity.ToTable("SkillProgression");

            entity.HasIndex(e => e.LearnerId, "IX_SkillProgression_LearnerID");

            entity.Property(e => e.ProgressId)
                .ValueGeneratedNever()
                .HasColumnName("ProgressID");
            entity.Property(e => e.LearnerId).HasColumnName("LearnerID");
            entity.Property(e => e.ProficiencyLevel)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SpecificSkill)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Specific_Skill");

            entity.HasOne(d => d.Learner).WithMany(p => p.SkillProgressions)
                .HasForeignKey(d => d.LearnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SkillProg__Learn__04E4BC85");
        });

        modelBuilder.Entity<Survey>(entity =>
        {
            entity.HasKey(e => e.SurveyId).HasName("PK__Survey__A5481F9DA746892B");

            entity.ToTable("Survey");

            entity.Property(e => e.SurveyId)
                .ValueGeneratedNever()
                .HasColumnName("SurveyID");
            entity.Property(e => e.ResponseTimestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SurveyQuestion>(entity =>
        {
            entity.HasKey(e => new { e.SurveyId, e.Question }).HasName("PK__SurveyQu__23FB983B6909533D");

            entity.Property(e => e.SurveyId).HasColumnName("SurveyID");
            entity.Property(e => e.Question)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Survey).WithMany(p => p.SurveyQuestions)
                .HasForeignKey(d => d.SurveyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SurveyQue__Surve__208CD6FA");
        });

        modelBuilder.Entity<TakenAssessment>(entity =>
        {
            entity.HasKey(e => new { e.AssessmentId, e.LearnerId }).HasName("PK__TakenAss__8B5147F11953F770");

            entity.ToTable("TakenAssessment");

            entity.HasIndex(e => e.LearnerId, "IX_TakenAssessment_LearnerID");

            entity.Property(e => e.AssessmentId).HasColumnName("AssessmentID");
            entity.Property(e => e.LearnerId).HasColumnName("LearnerID");

            entity.HasOne(d => d.Assessment).WithMany(p => p.TakenAssessments)
                .HasForeignKey(d => d.AssessmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TakenAsse__Asses__0A9D95DB");

            entity.HasOne(d => d.Learner).WithMany(p => p.TakenAssessments)
                .HasForeignKey(d => d.LearnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TakenAsse__Learn__0B91BA14");
        });

        modelBuilder.Entity<TargetTrait>(entity =>
        {
            entity.HasKey(e => new { e.ModuleId, e.Trait }).HasName("PK__TargetTr__B51B9AB7C8818EC9");

            entity.Property(e => e.ModuleId).HasColumnName("ModuleID");
            entity.Property(e => e.Trait)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Module).WithMany(p => p.TargetTraits)
                .HasForeignKey(d => d.ModuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TargetTra__Modul__534D60F1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
