using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.IServices;
using NaviriaAPI.Entities;
using NaviriaAPI.DTOs;
using NaviriaAPI.DTOs.FeaturesDTOs;
using Microsoft.AspNetCore.Authorization;

namespace NaviriaAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(
            IUserService userService,
            ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var qoutes = await _userService.GetAllAsync();
                return Ok(qoutes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get all users");
                return StatusCode(500, "Failed to get all users");
            }            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var User = await _userService.GetByIdAsync(id);
                if (User == null) return NotFound();
                return Ok(User);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get user by id");
                return StatusCode(500, "Failed to get user by id");
            }
            
        }

        [AllowAnonymous]
        [HttpPost("add")]
        public async Task<IActionResult> Create([FromBody] UserCreateDto UserDto)
        {
            try
            {
                var createdUser = await _userService.CreateAsync(UserDto);
                return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create user");
                return StatusCode(500, "Failed to create user");
            }    
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UserUpdateDto UserDto)
        {
            try
            {
                var updated = await _userService.UpdateAsync(id, UserDto);
                return updated ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update user with ID {0}", id);
                return StatusCode(500, $"Failed to get create user with ID {id}");
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var deleted = await _userService.DeleteAsync(id);
                return deleted ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete user with ID {0}", id);
                return StatusCode(500, $"Failed to delete create user with ID {id}");
            }
        }

        [HttpGet("ask-chat-gpt")]
        public async Task<IActionResult> AskChatGPT(string question)
        {
            try
            {
                var answer = await _userService.GetAiAnswerAsync(question);
                return Ok(answer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to ask chat gpt question: ", question);
                return StatusCode(500, $"Failed to ask chat gpt question: {question}");
            }
        }
    }
}
