using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHealthCompanion.Entities
{
    public class ChatSession
    {
        public long Id { get; set; }
        [ForeignKey("AIPlan")]
        public long? AIPlanId { get; set; }
        public string? Title { get; set; } 
        public DateTime? LastMessageAt { get; set; }
        public ICollection<ChatMessage>? Messages { get; set; }
        public AIPlan? AIPlan { get; set; }
    }
}
