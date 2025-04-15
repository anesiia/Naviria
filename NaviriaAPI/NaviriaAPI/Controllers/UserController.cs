using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.IServices;
using NaviriaAPI.IServices.ICloudStorage;
using NaviriaAPI.Entities;
using NaviriaAPI.DTOs;
using NaviriaAPI.DTOs.FeaturesDTOs;
using Microsoft.AspNetCore.Authorization;
using NaviriaAPI.Services.CloudStorage;

namespace NaviriaAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        public readonly ICloudinaryService _cloudinaryService;

        public UserController(
            IUserService userService,
            ILogger<UserController> logger,
            ICloudinaryService cloudinaryService)
        {
            _userService = userService;
            _logger = logger;
            _cloudinaryService = cloudinaryService;
        }
 
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
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("User ID is required.");

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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

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
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("User ID is required.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

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
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("User ID is required.");

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
            if (string.IsNullOrWhiteSpace(question))
                return BadRequest("Question is required.");

            try
            {
                var answer = await _userService.GetAiAnswerAsync(question);
                return Ok(answer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to ask chat gpt question: {0}", question);
                return StatusCode(500, $"Failed to ask chat gpt question: {question}");
            }
        }

        [AllowAnonymous]
        [HttpPost("upload-profile-photo")]
        public async Task<IActionResult> UploadProfilePhoto([FromForm] string userId, [FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            try
            {
                var answer = await _cloudinaryService.UploadImageAsync(userId, file);
                return Ok(answer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upload user photo");
                return StatusCode(500, $"Failed to upload user photo");
            }
        }

    }
}
