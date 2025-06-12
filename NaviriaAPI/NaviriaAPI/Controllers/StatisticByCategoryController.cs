using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.IServices.IStatisticServices;
using Microsoft.AspNetCore.Authorization;

namespace NaviriaAPI.Controllers
{
    /// <summary>
    /// Controller for category-based task statistics (pie chart data).
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticByCategoryController : ControllerBase
    {
        private readonly IStatisticsByCategoryService _taskStatsByCategoryService;
        private readonly ILogger<StatisticByCategoryController> _logger;

        public StatisticByCategoryController(
            IStatisticsByCategoryService taskStatsService,
            ILogger<StatisticByCategoryController> logger)
        {
            _taskStatsByCategoryService = taskStatsService;
            _logger = logger;
        }

        /// <summary>
        /// Gets pie chart statistics of the user's tasks distribution by categories.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <returns>Pie chart data for the user's tasks by categories.</returns>
        /// <response code="200">Returns the user's pie chart statistics.</response>
        /// <response code="400">If the user ID is missing.</response>
        /// <response code="404">If the user has no tasks.</response>
        /// <response code="500">If an error occurs.</response>
        [Authorize]
        [HttpGet("user/{userId}/piechart")]
        public async Task<IActionResult> GetUserPieChart(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("User ID is required.");

            try
            {
                var stats = await _taskStatsByCategoryService.GetUserPieChartStatsAsync(userId);
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get pie chart stats for user {UserId}", userId);
                return StatusCode(500, $"Failed to get pie chart stats for user {userId}");
            }
        }

        /// <summary>
        /// Gets pie chart statistics of tasks by categories for a user and their friends.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <returns>Pie chart data for the user and friends' tasks by categories.</returns>
        /// <response code="200">Returns the combined pie chart statistics.</response>
        /// <response code="400">If the user ID is missing.</response>
        /// <response code="404">If there are no tasks for the user and friends.</response>
        /// <response code="500">If an error occurs.</response>
        [Authorize]
        [HttpGet("user/{userId}/friends/piechart")]
        public async Task<IActionResult> GetUserAndFriendsPieChart(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("User ID is required.");

            try
            {
                var stats = await _taskStatsByCategoryService.GetUserAndFriendsPieChartStatsAsync(userId);
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get user and friends pie chart stats for user {UserId}", userId);
                return StatusCode(500, $"Failed to get user and friends pie chart stats for user {userId}");
            }
        }

        /// <summary>
        /// Gets global pie chart statistics of tasks by categories for all users.
        /// </summary>
        /// <returns>Pie chart data for all users' tasks by categories.</returns>
        /// <response code="200">Returns the global pie chart statistics.</response>
        /// <response code="500">If an error occurs.</response>
        [Authorize]
        [HttpGet("global/piechart")]
        public async Task<IActionResult> GetGlobalPieChart()
        {
            try
            {
                var stats = await _taskStatsByCategoryService.GetGlobalPieChartStatsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get global pie chart stats");
                return StatusCode(500, "Failed to get global pie chart stats");
            }
        }
    }
}
