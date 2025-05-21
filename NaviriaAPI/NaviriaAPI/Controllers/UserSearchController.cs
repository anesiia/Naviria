using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.IServices;
using NaviriaAPI.DTOs;
using NaviriaAPI.Exceptions;
using Microsoft.Extensions.Logging;

namespace NaviriaAPI.Controllers
{
    /// <summary>
    /// Provides search and filtering operations for users, friends, and incoming friend requests
    /// by optional category and/or name (nickname or full name).
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UserSearchController : ControllerBase
    {
        private readonly IUserSearchService _userSearchService;
        private readonly ILogger<UserSearchController> _logger;

        public UserSearchController(
            IUserSearchService userSearchService,
            ILogger<UserSearchController> logger)
        {
            _userSearchService = userSearchService;
            _logger = logger;
        }

        /// <summary>
        /// Search all users in the system (excluding the current user) with optional filtering by category and/or nickname or full name.
        /// </summary>
        /// <param name="userId">The ID of the user performing the search (excluded from results).</param>
        /// <param name="categoryId">Optional category ID to filter users who have at least one task in this category.</param>
        /// <param name="query">Optional search string for nickname or full name.</param>
        /// <returns>
        /// 200: List of users (<see cref="UserDto"/>) matching the criteria.<br/>
        /// 400: If <paramref name="userId"/> is not provided.<br/>
        /// 404: If no users found.<br/>
        /// 500: If an internal error occurs.
        /// </returns>
        [HttpGet("search-all")]
        public async Task<IActionResult> SearchAllUsers(
            [FromQuery] string userId,
            [FromQuery] string? categoryId = null,
            [FromQuery] string? query = null)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("User ID is required.");

            try
            {
                var users = await _userSearchService.SearchAllUsersAsync(userId, categoryId, query);
                return Ok(users);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "No users found for search-all for user {UserId}", userId);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to search all users for user {UserId}", userId);
                return StatusCode(500, "Failed to search users.");
            }
        }

        /// <summary>
        /// Search among the user's friends with optional filtering by category and/or nickname or full name.
        /// </summary>
        /// <param name="userId">The ID of the user performing the search.</param>
        /// <param name="categoryId">Optional category ID to filter friends who have at least one task in this category.</param>
        /// <param name="query">Optional search string for nickname or full name.</param>
        /// <returns>
        /// 200: List of friends (<see cref="UserDto"/>) matching the criteria.<br/>
        /// 400: If <paramref name="userId"/> is not provided.<br/>
        /// 404: If no friends found.<br/>
        /// 500: If an internal error occurs.
        /// </returns>
        [HttpGet("search-friends")]
        public async Task<IActionResult> SearchFriends(
            [FromQuery] string userId,
            [FromQuery] string? categoryId = null,
            [FromQuery] string? query = null)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("User ID is required.");

            try
            {
                var friends = await _userSearchService.SearchFriendsAsync(userId, categoryId, query);
                return Ok(friends);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "No friends found for user {UserId}", userId);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to search friends for user {UserId}", userId);
                return StatusCode(500, "Failed to search friends.");
            }
        }

        /// <summary>
        /// Search among users who have sent a friend request to the user, with optional filtering by category and/or nickname or full name.
        /// </summary>
        /// <param name="userId">The ID of the user performing the search.</param>
        /// <param name="categoryId">Optional category ID to filter users who have at least one task in this category.</param>
        /// <param name="query">Optional search string for nickname or full name.</param>
        /// <returns>
        /// 200: List of users (<see cref="UserDto"/>) who sent friend requests and match the criteria.<br/>
        /// 400: If <paramref name="userId"/> is not provided.<br/>
        /// 404: If no incoming friend requests found.<br/>
        /// 500: If an internal error occurs.
        /// </returns>
        [HttpGet("search-incoming-requests")]
        public async Task<IActionResult> SearchIncomingFriendRequests(
            [FromQuery] string userId,
            [FromQuery] string? categoryId = null,
            [FromQuery] string? query = null)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("User ID is required.");

            try
            {
                var requests = await _userSearchService.SearchIncomingFriendRequestsAsync(userId, categoryId, query);
                return Ok(requests);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "No incoming requests found for user {UserId}", userId);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to search incoming requests for user {UserId}", userId);
                return StatusCode(500, "Failed to search incoming friend requests.");
            }
        }
    }
}
