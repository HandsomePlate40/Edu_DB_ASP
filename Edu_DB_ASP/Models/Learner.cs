using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class Learner
{
    public int LearnerId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Gender { get; set; }

    public string? CountryOfOrigin { get; set; }

    public string? CulturalBackground { get; set; }

    public string? PersonalityTraits { get; set; }

    public string? EmotionalProfile { get; set; }

    public string? PhysicalHealth { get; set; }

    public string? MentalHealth { get; set; }

    public string? ExperienceLevel { get; set; }

    public string? Email { get; set; }

    public string PasswordHash { get; set; } = null!;

    public virtual ICollection<Achievement> Achievements { get; set; } = new List<Achievement>();

    public virtual ICollection<CourseEnrollment> CourseEnrollments { get; set; } = new List<CourseEnrollment>();

    public virtual ICollection<EmotionalFeedback> EmotionalFeedbacks { get; set; } = new List<EmotionalFeedback>();

    public virtual ICollection<Express> Expresses { get; set; } = new List<Express>();

    public virtual ICollection<InteractionLog> InteractionLogs { get; set; } = new List<InteractionLog>();

    public virtual ICollection<Join> Joins { get; set; } = new List<Join>();

    public virtual ICollection<LeaderboardRank> LeaderboardRanks { get; set; } = new List<LeaderboardRank>();

    public virtual ICollection<LeaderboardStudentsCourse> LeaderboardStudentsCourses { get; set; } = new List<LeaderboardStudentsCourse>();

    public virtual ICollection<LearnerSkill> LearnerSkills { get; set; } = new List<LearnerSkill>();

    public virtual ICollection<LearningGoal> LearningGoals { get; set; } = new List<LearningGoal>();

    public virtual ICollection<LearningPreference> LearningPreferences { get; set; } = new List<LearningPreference>();

    public virtual ICollection<PersonalizationProfile> PersonalizationProfiles { get; set; } = new List<PersonalizationProfile>();

    public virtual ICollection<SkillProgression> SkillProgressions { get; set; } = new List<SkillProgression>();

    public virtual ICollection<TakenAssessment> TakenAssessments { get; set; } = new List<TakenAssessment>();

    public virtual ICollection<Achievement> AchievementsNavigation { get; set; } = new List<Achievement>();

    public virtual ICollection<Badge> Badges { get; set; } = new List<Badge>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Quest> Quests { get; set; } = new List<Quest>();

    public virtual ICollection<Survey> Surveys { get; set; } = new List<Survey>();
}
