namespace SmartHealthCompanion.Entities
{
    public class BehaviorLog
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public DateTime LogDate { get; set; }
        public bool? FollowedDiet { get; set; }
        public bool? CompletedWorkout { get; set; }
        public bool? WaterGoalMet { get; set; }
        public decimal? SleepHours { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}
