using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.IServices;
using NaviriaAPI.DTOs.FeaturesDTOs;
using NaviriaAPI.Services;

namespace NaviriaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssistantChatController : ControllerBase
    {
        private readonly IAssistantChatService _chatService;

        public AssistantChatController(IAssistantChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetChatForUser(string userId)
        {
            var messages = await _chatService.GetUserChatAsync(userId);
            return Ok(messages);
        }


        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] AssistantChatMessageDto dto)
        {
            var reply = await _chatService.SendMessageAsync(dto);
            return Ok(new { response = reply });
        }
    }
}
