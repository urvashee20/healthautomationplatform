namespace SmartHealthCompanion.Entities
{
    public class Notification
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string? Message { get; set; }
        public bool? IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public UserProfile UserProfile { get; set; }
    }
}
