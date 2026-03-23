namespace SmartHealthCompanion.DTOs
{
    public class CommonDto
    {
        public class RegisterDto
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
        public class LoginDto
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
        public class CreateGoalDto
        {
            public string? PrimaryGoal { get; set; }
            public List<string>? SecondaryGoals { get; set; }
            public List<string>? HealthConditions { get; set; }
            public bool IsCustomGoal { get; set; }
            public string? CustomGoalText { get; set; }
            public decimal? TargetWeight { get; set; }
            public int? DurationInDays { get; set; }
            public bool? HasGymAccess { get; set; }
            public decimal? SleepHours { get; set; }
        }
    }
}
