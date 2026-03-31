namespace SmartHealthCompanion.DTOs.Chat
{
    public class ChatResponseDto
    {
        public string UserMessage { get; set; } = string.Empty;
        public string? AIResponse { get; set; }
        public double? ResponseTimeMs { get; set; }
    }
}
