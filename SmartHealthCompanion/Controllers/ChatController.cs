using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartHealthCompanion.DTOs.Chat;
using SmartHealthCompanion.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHealthCompanion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly ILogger<ChatController> _logger;

        public ChatController(IChatService chatService, ILogger<ChatController> logger)
        {
            _chatService = chatService;
            _logger = logger;
        }

        [HttpPost("session/create")]
        public async Task<ActionResult<ChatSessionDto>> CreateSession([FromBody] CreateChatSessionDto request)
        {
            try
            {
                var session = await _chatService.CreateSessionAsync(request.UserProfileId);
                return Ok(session);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating chat session for UserProfileId: {UserProfileId}", request.UserProfileId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("session/{userProfileId}")]
        public async Task<ActionResult<IEnumerable<ChatSessionDto>>> GetUserSessions(long userProfileId)
        {
            try
            {
                var sessions = await _chatService.GetUserSessionsAsync(userProfileId);
                return Ok(sessions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching chat sessions for UserProfileId: {UserProfileId}", userProfileId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost("send")]
        public async Task<ActionResult<ChatResponseDto>> SendMessage([FromBody] SendMessageDto request)
        {
            try
            {
                var response = await _chatService.SendMessageAsync(request);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message in ChatSessionId: {ChatSessionId}", request.ChatSessionId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("messages/{chatSessionId}")]
        public async Task<ActionResult<IEnumerable<ChatMessageDto>>> GetSessionMessages(long chatSessionId)
        {
            try
            {
                var messages = await _chatService.GetSessionMessagesAsync(chatSessionId);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching chat messages for ChatSessionId: {ChatSessionId}", chatSessionId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("session/{chatSessionId}")]
        public async Task<IActionResult> DeleteSession(long chatSessionId)
        {
            try
            {
                var result = await _chatService.DeleteSessionAsync(chatSessionId);
                if (!result)
                    return NotFound($"Chat session with ID {chatSessionId} not found.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting chat session for ChatSessionId: {ChatSessionId}", chatSessionId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
