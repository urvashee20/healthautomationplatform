namespace SmartHealthCompanion.DTOs
{
    public class AIPlanDto 
    {
        public string DietPlan { get; set; }
        public string WorkoutPlan { get; set; }
        public string WaterPlan { get; set; }
        public string SleepPlan { get; set; }
    }

    public class AIPlanResponseDto
    {
        public long Id { get; set; }
        public string DietPlan { get; set; }
        public string WorkoutPlan { get; set; }
        public string WaterPlan { get; set; }
        public string SleepPlan { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
