using System.Collections.Generic;
using System.Threading.Tasks;
using SmartHealthCompanion.DTOs.Chat;

namespace SmartHealthCompanion.Interfaces
{
    public interface IChatService
    {
        Task<ChatSessionDto> CreateSessionAsync(long userProfileId);
        Task<IEnumerable<ChatSessionDto>> GetUserSessionsAsync(long userProfileId);
        Task<ChatResponseDto> SendMessageAsync(SendMessageDto request);
        Task<IEnumerable<ChatMessageDto>> GetSessionMessagesAsync(long chatSessionId);
        Task<bool> DeleteSessionAsync(long chatSessionId);
    }
}
