using static SmartHealthCompanion.Enum;

namespace SmartHealthCompanion.Entities
{
    public class Goal
    {
        public long Id { get; set; }
        public long UserProfileId { get; set; }
        public string? PrimaryGoal { get; set; }
        public string? SecondaryGoals { get; set; }
        public string? HealthConditions { get; set; }
        public bool IsCustomGoal { get; set; }
        public string? CustomGoalText { get; set; }
        public decimal? TargetWeight { get; set; }
        public int? DurationInDays { get; set; }
        public bool? HasGymAccess { get; set; }
        public decimal? SleepHours { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public UserProfile UserProfile { get; set; }
    }
}
