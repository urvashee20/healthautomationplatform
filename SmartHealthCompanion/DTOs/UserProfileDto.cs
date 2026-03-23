namespace SmartHealthCompanion.DTOs
{
    public class UserProfileDto
    {
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public decimal? Height { get; set; }
        public decimal Weight { get; set; }
        public string? JobType { get; set; }
        public string? JobName { get; set; }
        public DateTime? JobStartTime { get; set; }
        public DateTime? JobEndTime { get; set; }
        public string DailyRoutine { get; set; }
        public string FoodPreference { get; set; } // Veg / NonVeg
        public string? HealthConditions { get; set; } // Diabetes, BP
        public decimal? SleepHours { get; set; }
    }
}
