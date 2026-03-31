using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartHealthCompanion.Data;
using SmartHealthCompanion.DTOs.Chat;
using SmartHealthCompanion.Entities;
using SmartHealthCompanion.Interfaces;

namespace SmartHealthCompanion.Services
{
    public class ChatService : IChatService
    {
        private readonly AppDbContext _context;
        private readonly IAIService _aiService;
        private readonly ILogger<ChatService> _logger;

        public ChatService(AppDbContext context, IAIService aiService, ILogger<ChatService> logger)
        {
            _context = context;
            _aiService = aiService;
            _logger = logger;
        }

        public async Task<ChatSessionDto> CreateSessionAsync(long userProfileId)
        {
            var session = new ChatSession
            {
                UserProfileId = userProfileId,
                Title = "New Chat",
                LastMessageAt = DateTime.UtcNow
            };

            _context.ChatSession.Add(session);
            await _context.SaveChangesAsync();

            return MapToSessionDto(session);
        }

        public async Task<IEnumerable<ChatSessionDto>> GetUserSessionsAsync(long userProfileId)
        {
            return await _context.ChatSession
                .Where(cs => cs.UserProfileId == userProfileId)
                .OrderByDescending(cs => cs.LastMessageAt)
                .Select(cs => new ChatSessionDto
                {
                    Id = cs.Id,
                    UserProfileId = cs.UserProfileId,
                    Title = cs.Title,
                    LastMessageAt = cs.LastMessageAt
                })
                .ToListAsync();
        }

        public async Task<ChatResponseDto> SendMessageAsync(SendMessageDto request)
        {
            var session = await _context.ChatSession
                .FirstOrDefaultAsync(cs => cs.Id == request.ChatSessionId);

            if (session == null)
            {
                throw new KeyNotFoundException($"Chat session with ID {request.ChatSessionId} not found.");
            }

            var userMessageTrimmed = request.Message?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(userMessageTrimmed))
            {
                throw new ArgumentException("Message cannot be empty.");
            }

            // Auto-generate title from first message
            if (string.IsNullOrEmpty(session.Title) || session.Title == "New Chat")
            {
                session.Title = userMessageTrimmed.Length > 30 
                    ? userMessageTrimmed.Substring(0, 30) + "..." 
                    : userMessageTrimmed;
            }

            // Load last 5 messages for AI context
            var previousMessages = await _context.ChatMessage
                .Where(cm => cm.ChatSessionId == request.ChatSessionId && !cm.IsError)
                .OrderByDescending(cm => cm.CreatedAt)
                .Take(5)
                .ToListAsync();
            
            // Reverse so they are chronological
            previousMessages.Reverse();

            var chatMessage = new ChatMessage
            {
                ChatSessionId = request.ChatSessionId,
                UserMessage = userMessageTrimmed,
                CreatedAt = DateTime.UtcNow,
                IsError = false
            };
            
            _context.ChatMessage.Add(chatMessage);
            await _context.SaveChangesAsync(); // Save user message immediately

            var stopwatch = Stopwatch.StartNew();
            string aiResponse = null;
            bool isError = false;

            try
            {
                aiResponse = await _aiService.GetChatResponseAsync(userMessageTrimmed, previousMessages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get AI response for ChatSessionId: {ChatSessionId}", request.ChatSessionId);
                isError = true;
                aiResponse = "An error occurred while communicating with the AI. Please try again later.";
            }
            finally
            {
                stopwatch.Stop();
            }

            chatMessage.AIResponse = aiResponse;
            chatMessage.IsError = isError;
            chatMessage.ResponseTimeMs = stopwatch.Elapsed.TotalMilliseconds;
            
            session.LastMessageAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new ChatResponseDto
            {
                UserMessage = chatMessage.UserMessage,
                AIResponse = chatMessage.AIResponse,
                ResponseTimeMs = chatMessage.ResponseTimeMs
            };
        }

        public async Task<IEnumerable<ChatMessageDto>> GetSessionMessagesAsync(long chatSessionId)
        {
            return await _context.ChatMessage
                .Where(cm => cm.ChatSessionId == chatSessionId)
                .OrderBy(cm => cm.CreatedAt)
                .Select(cm => new ChatMessageDto
                {
                    Id = cm.Id,
                    ChatSessionId = cm.ChatSessionId,
                    UserMessage = cm.UserMessage,
                    AIResponse = cm.AIResponse,
                    IsError = cm.IsError,
                    ResponseTimeMs = cm.ResponseTimeMs,
                    CreatedAt = cm.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<bool> DeleteSessionAsync(long chatSessionId)
        {
            var session = await _context.ChatSession
                .Include(cs => cs.Messages)
                .FirstOrDefaultAsync(cs => cs.Id == chatSessionId);

            if (session == null)
            {
                return false;
            }

            _context.ChatSession.Remove(session);
            await _context.SaveChangesAsync();
            return true;
        }

        private ChatSessionDto MapToSessionDto(ChatSession session)
        {
            return new ChatSessionDto
            {
                Id = session.Id,
                UserProfileId = session.UserProfileId,
                Title = session.Title,
                LastMessageAt = session.LastMessageAt
            };
        }
    }
}
