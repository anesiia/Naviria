using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.IServices.IUserServices;
using NaviriaAPI.Exceptions;
using Microsoft.Extensions.Logging;
using NaviriaAPI.IServices;
using NaviriaAPI.DTOs;

namespace NaviriaAPI.Controllers
{
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
        /// Get all users who have at least one task in the given category.
        /// </summary>
        /// <param name="categoryId">The ID of the category.</param>
        /// <returns>
        /// 200: List of users (<see cref="UserDto"/>) who have at least one task in this category.<br/>
        /// 400: If the categoryId is not provided.<br/>
        /// 404: If no users are found for this category.<br/>
        /// 500: If an internal error occurs.
        /// </returns>
        [HttpGet("users/by-category/{categoryId}")]
        public async Task<IActionResult> GetUsersByTaskCategory(string categoryId)
        {
            if (string.IsNullOrWhiteSpace(categoryId))
                return BadRequest("Category ID is required.");

            try
            {
                var result = await _userSearchService.GetUsersByTaskCategoryAsync(categoryId);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "No users found for category {CategoryId}", categoryId);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to search users by category {CategoryId}", categoryId);
                return StatusCode(500, "Failed to search users by category.");
            }
        }

        /// <summary>
        /// Get all users' potentional friends who have at least one task in the given category.
        /// </summary>
        /// <param name="categoryId">The ID of the category.</param>
        /// /// <param name="userId">The ID of the user.</param>
        /// <returns>
        /// 200: List of users (<see cref="UserDto"/>) who have at least one task in this category.<br/>
        /// 400: If the categoryId is not provided.<br/>
        /// 404: If no users are found for this category.<br/>
        /// 500: If an internal error occurs.
        /// </returns>
        [HttpGet("potential-friends/by-category/{categoryId}")]
        public async Task<IActionResult> GetPotentialFriendsByTaskByCategory([FromQuery] string userId, string categoryId)
        {
            if (string.IsNullOrWhiteSpace(categoryId))
                return BadRequest("Category ID is required.");

            try
            {
                var result = await _userSearchService.GetPotentialFriendsByTaskCategoryAsync(userId, categoryId);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "No users found for category {CategoryId}", categoryId);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to search users by category {CategoryId}", categoryId);
                return StatusCode(500, "Failed to search users by category.");
            }
        }

        /// <summary>
        /// Search among potential friends by nickname.
        /// </summary>
        /// <param name="userId">The ID of the user who is searching.</param>
        /// <param name="query">Search string (part of nickname).</param>
        /// <returns>
        /// 200: List of users (<see cref="UserDto"/>) matching the query and not being friends with the user.<br/>
        /// 400: If userId or query is not provided.<br/>
        /// 500: If an internal error occurs.
        /// </returns>
        [HttpGet("potential-friends/search")]
        public async Task<IActionResult> SearchPotentialFriendsByNickname([FromQuery] string userId, [FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(query))
                return BadRequest("User ID and search query are required.");

            try
            {
                var result = await _userSearchService.SearchPotentialFriendsByNicknameAsync(userId, query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to search potential friends by nickname for user {UserId}", userId);
                return StatusCode(500, "Failed to search potential friends by nickname.");
            }
        }

        /// <summary>
        /// Search among user's friends by nickname.
        /// </summary>
        /// <param name="userId">The ID of the user who is searching.</param>
        /// <param name="query">Search string (part of nickname).</param>
        /// <returns>
        /// 200: List of user's friends (<see cref="UserDto"/>) matching the query.<br/>
        /// 400: If userId or query is not provided.<br/>
        /// 500: If an internal error occurs.
        /// </returns>
        [HttpGet("friends/search")]
        public async Task<IActionResult> SearchFriendsByNickname([FromQuery] string userId, [FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(query))
                return BadRequest("User ID and search query are required.");

            try
            {
                var result = await _userSearchService.SearchFriendsByNicknameAsync(userId, query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to search friends by nickname for user {UserId}", userId);
                return StatusCode(500, "Failed to search friends by nickname.");
            }
        }

        /// <summary>
        /// Search among users who sent a friend request to the specified user, by nickname.
        /// </summary>
        /// <param name="userId">The ID of the user who received friend requests.</param>
        /// <param name="query">Search string (part of nickname).</param>
        /// <returns>
        /// 200: List of users (<see cref="UserDto"/>) who sent a friend request and match the query.<br/>
        /// 400: If userId or query is not provided.<br/>
        /// 500: If an internal error occurs.
        /// </returns>
        [HttpGet("incoming-requests/search")]
        public async Task<IActionResult> SearchIncomingFriendRequestsByNickname([FromQuery] string userId, [FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(query))
                return BadRequest("User ID and search query are required.");

            try
            {
                var result = await _userSearchService.SearchIncomingFriendRequestsByNicknameAsync(userId, query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to search incoming friend requests by nickname for user {UserId}", userId);
                return StatusCode(500, "Failed to search incoming friend requests by nickname.");
            }
        }
    }
}
