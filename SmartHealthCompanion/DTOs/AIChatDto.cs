namespace SmartHealthCompanion.DTOs
{
    public class AIChatDto
    {
    }
    public class SendMessageDto
    {
        public long ChatSessionId { get; set; }
        public string Message { get; set; }
    }
    public class ChatResponseDto
    {
        public string UserMessage { get; set; }
        public string AIResponse { get; set; }
        public double ResponseTimeMs { get; set; }
    }
    public class ChatSessionDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime? LastMessageAt { get; set; }
    }
}
