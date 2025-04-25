using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.IServices;
using NaviriaAPI.IServices.ICloudStorage;

namespace NaviriaAPI.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FriendsController : ControllerBase
    {
        private readonly IFriendService _friendService;
        private readonly ILogger<FriendsController> _logger;
        public readonly ICloudinaryService _cloudinaryService;

        public FriendsController(
            ILogger<FriendsController> logger,
            ICloudinaryService cloudinaryService,
            IFriendService friendService)
        {
            _logger = logger;
            _cloudinaryService = cloudinaryService;
            _friendService = friendService;
        }


        [HttpGet("{id}")]
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

        [HttpDelete("{fromUserId}/to/{friendId}")]
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

        [HttpGet("{userId}/search-friends")]
        public async Task<IActionResult> SearchFriendsByNickname(string userId, [FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Search query is required.");

            try
            {
                var results = await _friendService.SearchUsersByNicknameAsync(userId, query);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to search users by nickname for user {UserId}", userId);
                return StatusCode(500, "Error searching for users.");
            }
        }
    }
}
