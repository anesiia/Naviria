using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.IServices;
using NaviriaAPI.IServices.ICloudStorage;
using NaviriaAPI.Entities;
using NaviriaAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using NaviriaAPI.Services.CloudStorage;
using Newtonsoft.Json.Linq;

namespace NaviriaAPI.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IFriendService _friendService;
        private readonly ILogger<UserController> _logger;
        public readonly ICloudinaryService _cloudinaryService;

        public UserController(
            IUserService userService,
            ILogger<UserController> logger,
            ICloudinaryService cloudinaryService,
            IFriendService friendService)
        {
            _userService = userService;
            _logger = logger;
            _cloudinaryService = cloudinaryService;
            _friendService = friendService;
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
                return StatusCode(500, $"Failed to get create user with ID {id}");
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

        [HttpGet("{id}/friends")]
        public async Task<IActionResult> GetFriends(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("User ID is required.");

            try
            {
                var friends = await _friendService.GetUserFriendsAsync(id);
                return Ok(friends);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get friends for user {Id}", id);
                return StatusCode(500, $"Failed to get friends for user {id}");
            }
        }

        [HttpDelete("{fromUserId}/friends/{friendId}")]
        public async Task<IActionResult> DeleteFriend(string fromUserId, string friendId)
        {
            if (string.IsNullOrWhiteSpace(fromUserId))
                return BadRequest("User ID is required.");
            if (string.IsNullOrWhiteSpace(friendId))
                return BadRequest("Friend ID is required.");

            try
            {
                var deleted = await _friendService.DeleteFriendAsync(fromUserId, friendId);
                return deleted ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete friend of user with ID {0}", fromUserId);
                return StatusCode(500, $"Failed to delete friend of user with ID {fromUserId}");
            }
        }

        [HttpGet("{id}/potential-friends")]
        public async Task<IActionResult> GetPotentialFriends(string id)
        {
            try
            {
                var users = await _friendService.GetPotentialFriendsAsync(id);
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get potential friends for user {UserId}", id);
                return StatusCode(500, "An error occurred while retrieving potential friends.");
            }
        }

    }
}
