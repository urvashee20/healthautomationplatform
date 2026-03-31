using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHealthCompanion.Entities
{
    public class ChatSession
    {
        public long Id { get; set; }

        [ForeignKey("UserProfile")]
        public long? UserProfileId { get; set; }
        public string? Title { get; set; } 
        public DateTime? LastMessageAt { get; set; }
        public ICollection<ChatMessage>? Messages { get; set; }
        public UserProfile? UserProfile { get; set; }
    }
}
