using static SmartHealthCompanion.Enum;

namespace SmartHealthCompanion.Entities
{
    public class Goal
    {
        public long Id { get; set; }
        public long UserProfileId { get; set; }
        public string GoalType { get; set; }
        public decimal? TargetWeight { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public UserProfile UserProfile { get; set; }
    }
}
