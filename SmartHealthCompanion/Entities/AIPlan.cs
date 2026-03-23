namespace SmartHealthCompanion.Entities
{
    public class AIPlan
    {
        public long Id { get; set; }
        public long UserProfileId { get; set; }
        public string DietPlan { get; set; }
        public string WorkoutPlan { get; set; }
        public string WaterPlan { get; set; }
        public string SleepPlan { get; set; }
        public string? Notes { get; set; } // extra AI suggestions
        public DateTime CreatedAt { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}
