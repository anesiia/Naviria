using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.IServices;
using NaviriaAPI.DTOs.FeaturesDTOs;

namespace NaviriaAPI.Controllers
{
    public class AssistantChatController : ControllerBase
    {
        private readonly IAssistantChatService _chatService;

        public AssistantChatController(IAssistantChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] AssistantChatRequestDto dto)
        {
            var reply = await _chatService.SendMessageAsync(dto.UserId, dto.Message);
            return Ok(new { response = reply });
        }
    }
}
