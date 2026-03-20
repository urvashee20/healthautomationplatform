namespace SmartHealthCompanion.Entities
{
    public class UserProfile
    {
        public long Id { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public decimal? Height { get; set; } // in cm
        public decimal Weight { get; set; } // current weight
        public string? JobType { get; set; } // Sitting / Active
        public string? JobName { get; set; }
        public DateTime? JobStartTime { get; set; }
        public DateTime? JobEndTime { get; set; }
        public string DailyRoutine { get; set; }
        public User User { get; set; }
        public ICollection<Goal>? Goals { get; set; }
        public ICollection<DailyLog>? DailyLogs { get; set; }
        public ICollection<Recommendation>? Recommendations { get; set; }
        public ICollection<BehaviorLog>? BehaviorLogs { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
    }
}
