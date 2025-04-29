using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.IServices;
using NaviriaAPI.IServices.ICloudStorage;
using Microsoft.AspNetCore.Authorization;

namespace NaviriaAPI.Controllers
{
    //[Authorize]
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

        [AllowAnonymous]
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

        [AllowAnonymous]
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
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserCreateDto UserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var token = await _userService.CreateAsync(UserDto);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create user");
                return StatusCode(500, "Failed to create user");
            }    
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
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
                return StatusCode(500, $"Failed to get update user with ID {id}");
            }
        }

        [HttpDelete("{id}")]
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
                return StatusCode(500, $"Failed to delete user with ID {id}");
            }
        }

        [HttpPut("user/{userId}/achievement/{achievementId}")]
        public async Task<IActionResult> GiveAchievementToUser(string userId, string achievementId)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(achievementId))
                return BadRequest("User ID and Achievement ID are required.");

            try
            {
                var updated = await _userService.GiveAchievementAsync(userId, achievementId);
                return updated ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to give achievement user with ID {0}", userId);
                return StatusCode(500, $"Failed to get give achievement user with ID {userId}");
            }
        }
    }
}
