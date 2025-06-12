using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.IServices;
using NaviriaAPI.IServices.ICloudStorage;

namespace NaviriaAPI.Controllers
{
    /// <summary>
    /// Controller for managing user friendships, searching friends, and finding shared interests.
    /// </summary>
    [Authorize]
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

        /// <summary>
        /// Gets the list of friends for the specified user.
        /// </summary>
        /// <param name="id">The user's unique identifier.</param>
        /// <returns>A list of user's friends.</returns>
        /// <response code="200">Returns the list of friends.</response>
        /// <response code="400">If user ID is missing.</response>
        /// <response code="500">If an error occurs while retrieving friends.</response>
        [Authorize]
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

        /// <summary>
        /// Removes a friend connection between two users.
        /// </summary>
        /// <param name="fromUserId">The ID of the user who removes the friend.</param>
        /// <param name="friendId">The ID of the friend to be removed.</param>
        /// <returns>No content if deleted, not found if no such friend connection exists.</returns>
        /// <response code="204">If the friend was deleted successfully.</response>
        /// <response code="400">If either ID is missing.</response>
        /// <response code="500">If an error occurs during deletion.</response>
        [Authorize]
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

        /// <summary>
        /// Gets a list of potential friends for the user (users who are not already friends).
        /// </summary>
        /// <param name="id">The user's unique identifier.</param>
        /// <returns>A list of users who are not currently friends with the specified user.</returns>
        /// <response code="200">Returns the list of potential friends.</response>
        /// <response code="500">If an error occurs while retrieving potential friends.</response>
        [Authorize]
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

        /// <summary>
        /// Searches for users by nickname, excluding the user's current friends and themselves.
        /// </summary>
        /// <param name="userId">The user's unique identifier.</param>
        /// <param name="query">The nickname to search for.</param>
        /// <returns>A list of users matching the search criteria.</returns>
        /// <response code="200">Returns the list of found users.</response>
        /// <response code="400">If the search query is empty.</response>
        /// <response code="500">If an error occurs while searching for users.</response>
        [Authorize]
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

        /// <summary>
        /// Finds shared interests (task categories and tags) between two users.
        /// </summary>
        /// <param name="userId1">The first user's unique identifier.</param>
        /// <param name="userId2">The second user's unique identifier.</param>
        /// <returns>
        /// An object containing lists of shared category names and tag names.
        /// </returns>
        /// <response code="200">Returns the shared interests.</response>
        /// <response code="400">If either user ID is missing.</response>
        /// <response code="500">If an error occurs while retrieving shared interests.</response>
        [Authorize]
        [HttpGet("shared-interests/{userId1}/{userId2}")]
        public async Task<IActionResult> GetSharedInterests(string userId1, string userId2)
        {
            if (string.IsNullOrWhiteSpace(userId1) || string.IsNullOrWhiteSpace(userId2))
                return BadRequest("Both user IDs are required.");

            try
            {
                var result = await _friendService.GetSharedInterestsAsync(userId1, userId2);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get shared interests for users {UserId1} and {UserId2}", userId1, userId2);
                return StatusCode(500, "Failed to get shared interests");
            }
        }
    }
}
