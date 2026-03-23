namespace SmartHealthCompanion.Entities
{
    public class HealthRisk
    {
        public long Id { get; set; }
        public long UserProfileId { get; set; }
        public string RiskType { get; set; } // Diabetes, Heart
        public string RiskLevel { get; set; } // Low, Medium, High
        public string Suggestion { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public UserProfile UserProfile { get; set; }
    }
}
