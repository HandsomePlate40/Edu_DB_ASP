using System;
using System.Collections.Generic;

namespace Edu_DB_ASP.Models;

public partial class Quest
{
    public int QuestId { get; set; }

    public string Title { get; set; } = null!;

    public string? QuestDescription { get; set; }

    public string? DifficultyLevel { get; set; }

    public string? QuestType { get; set; }

    public string? Criteria { get; set; }

    public virtual CollaborativeQuest? CollaborativeQuest { get; set; }

    public virtual ICollection<QuestReward> QuestRewards { get; set; } = new List<QuestReward>();

    public virtual SkillMasteryQuest? SkillMasteryQuest { get; set; }

    public virtual ICollection<Learner> Learners { get; set; } = new List<Learner>();

    public virtual ICollection<Reward> Rewards { get; set; } = new List<Reward>();
}
