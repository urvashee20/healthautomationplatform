namespace SmartHealthCompanion.DTOs.Chat
{
    public class SendMessageDto
    {
        public long ChatSessionId { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
