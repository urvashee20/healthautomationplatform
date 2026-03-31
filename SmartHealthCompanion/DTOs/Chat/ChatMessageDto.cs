using System;

namespace SmartHealthCompanion.DTOs.Chat
{
    public class ChatMessageDto
    {
        public long Id { get; set; }
        public long? ChatSessionId { get; set; }
        public string UserMessage { get; set; } = string.Empty;
        public string? AIResponse { get; set; }
        public bool IsError { get; set; }
        public double? ResponseTimeMs { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
