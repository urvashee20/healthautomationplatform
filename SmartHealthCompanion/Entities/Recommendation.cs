namespace SmartHealthCompanion.Entities
{
    public class Recommendation
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public DateTime? RecommendationDate { get; set; }
        public string? DietPlan { get; set; }
        public string? WorkoutPlan { get; set; }
        public string? WaterSuggestion { get; set; }
        public bool? GeneratedByAI { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public UserProfile UserProfile { get; set; }
    }
}
