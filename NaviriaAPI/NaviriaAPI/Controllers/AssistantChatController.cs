using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.IServices;
using NaviriaAPI.DTOs.FeaturesDTOs;
using Microsoft.Extensions.Logging;

namespace NaviriaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssistantChatController : ControllerBase
    {
        private readonly IAssistantChatService _chatService;
        private readonly ILogger<AssistantChatController> _logger;

        public AssistantChatController(
            IAssistantChatService chatService,
            ILogger<AssistantChatController> logger)
        {
            _chatService = chatService;
            _logger = logger;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetChatForUser(string userId)
        {
            try
            {
                var messages = await _chatService.GetUserChatAsync(userId);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve assistant chat for user {UserId}", userId);
                return StatusCode(500, "An error occurred while retrieving chat.");
            }
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] AssistantChatMessageDto dto)
        {
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
