using System;

namespace SmartHealthCompanion.DTOs.Chat
{
    public class ChatSessionDto
    {
        public long Id { get; set; }
        public long? UserProfileId { get; set; }
        public string? Title { get; set; }
        public DateTime? LastMessageAt { get; set; }
    }
}
