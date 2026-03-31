using System.Collections.Generic;
using System.Threading.Tasks;
using SmartHealthCompanion.Entities;

namespace SmartHealthCompanion.Interfaces
{
    public interface IAIService
    {
        /// <summary>
        /// Gets a response from the AI model using the current message and previous context.
        /// </summary>
        Task<string> GetChatResponseAsync(string userMessage, IEnumerable<ChatMessage> previousMessages);
    }
}
