namespace SmartHealthCompanion.Entities
{
    public class DailyLog
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public DateTime LogDate { get; set; }
        public int BreakfastCalories { get; set; }
        public int LunchCalories { get; set; }
        public int DinnerCalories { get; set; }
        public int SnacksCalories { get; set; }
        public int WaterIntake { get; set; } 
        public int Steps { get; set; }
        public int OfficeHours { get; set; }
        public decimal Weight { get; set; }
        public int TotalCalories { get; set; }
        public int BurnedCalories { get; set; }
        public int NetCalories { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}
