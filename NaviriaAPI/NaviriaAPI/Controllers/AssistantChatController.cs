using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.IServices;
using NaviriaAPI.DTOs.FeaturesDTOs;
using Microsoft.Extensions.Logging;

namespace NaviriaAPI.Controllers
{
    /// <summary>
    /// API controller for interaction with the Assistant Chat (AI-powered chat).
    /// Provides endpoints to get the chat history for a user and to send a new message to the assistant.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AssistantChatController : ControllerBase
    {
        private readonly IAssistantChatService _chatService;
        private readonly ILogger<AssistantChatController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssistantChatController"/> class.
        /// </summary>
        /// <param name="chatService">Service for assistant chat operations.</param>
        /// <param name="logger">Logger instance.</param>
        public AssistantChatController(
            IAssistantChatService chatService,
            ILogger<AssistantChatController> logger)
        {
            _chatService = chatService;
            _logger = logger;
        }

        /// <summary>
        /// Gets the full assistant chat history for a specific user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>
        /// 200: Returns a list of assistant chat messages for the user.<br/>
        /// 400: If the user ID is missing.<br/>
        /// 404: If the user has no chat history.<br/>
        /// 500: If an error occurs while retrieving the chat.
        /// </returns>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetChatForUser(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("User ID is required.");

            try
            {
                var messages = await _chatService.GetUserChatAsync(userId);
                if (messages == null || !messages.Any())
                    return NotFound($"No chat history found for user {userId}.");
                return Ok(messages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve assistant chat for user {UserId}", userId);
                return StatusCode(500, "An error occurred while retrieving chat.");
            }
        }

        /// <summary>
        /// Sends a new message to the assistant and receives a response.
        /// </summary>
        /// <param name="dto">The assistant chat message DTO (includes user ID, message content, etc.).</param>
        /// <returns>
        /// 200: Returns the assistant's reply to the user message.<br/>
        /// 400: If the request body or user ID is invalid.<br/>
        /// 500: If an error occurs while processing the message.
        /// </returns>
        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] AssistantChatMessageDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.UserId))
                return BadRequest("Request body or User ID is missing.");

            try
            {
                var reply = await _chatService.SendMessageAsync(dto);
                return Ok(new { response = reply });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process chat message for user {UserId}", dto.UserId);
                return StatusCode(500, "An error occurred while processing the message.");
            }
        }
    }
}
