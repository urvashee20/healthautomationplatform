using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHealthCompanion.Entities
{
    public class ChatMessage
    {
        public long Id { get; set; }
        public long? ChatSessionId { get; set; }
        public string UserMessage { get; set; }
        public string? AIResponse { get; set; } 
        public bool IsError { get; set; }
        public double? ResponseTimeMs { get; set; }
        public DateTime CreatedAt { get; set; }
        public ChatSession? ChatSession { get; set; }
    }
}
