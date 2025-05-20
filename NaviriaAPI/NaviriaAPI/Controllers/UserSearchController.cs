using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.IServices.IUserServices;
using NaviriaAPI.Exceptions;
using NaviriaAPI.IServices;

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
        /// <param name="categoryId">Category ID</param>
        /// <returns>List of users</returns>
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
    }
}
