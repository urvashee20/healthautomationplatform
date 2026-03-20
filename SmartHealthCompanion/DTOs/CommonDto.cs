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
        public class GoalDto
        {
            public string GoalType { get; set; } 
            public decimal? TargetWeight { get; set; }
        }
    }
}
