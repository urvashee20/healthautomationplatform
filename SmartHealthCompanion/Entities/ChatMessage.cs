namespace SmartHealthCompanion.Entities
{
    public class ChatMessage
    {
        public long? Id { get; set; }
        public long? ChatSessionId { get; set; }
        public string Message { get; set; }
        public string? Response { get; set; } 
        public bool IsError { get; set; }
        public double? ResponseTimeMs { get; set; }
        public ChatSession? ChatSession { get; set; }
    }
}
