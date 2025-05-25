using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.IServices.IStatisticServices;

namespace NaviriaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsTaskByDateController : ControllerBase
    {
        private readonly ITaskStatisticByDateService _taskStatisticByDateService;
        private readonly ILogger<StatisticsTaskByDateController> _logger;

        public StatisticsTaskByDateController(ITaskStatisticByDateService service, ILogger<StatisticsTaskByDateController> logger)
        {
            _taskStatisticByDateService = service;
            _logger = logger;
        }

        /// <summary>
        /// Gets the user's statistics for completed tasks per month (line chart data).
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <returns>Line chart data for completed tasks per month by the user.</returns>
        /// <response code="200">Returns the user's completed tasks per month statistics.</response>
        /// <response code="400">If the user ID is missing.</response>
        /// <response code="404">If the user has no completed tasks.</response>
        /// <response code="500">If an error occurs.</response>
        [HttpGet("user/{userId}/completed/monthly")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserTasksCompletedPerMonth(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("User ID is required.");

            try
            {
                var stats = await _taskStatisticByDateService.GetUserTasksCompletedPerMonthAsync(userId);
                if (stats == null || !stats.Any())
                    return NotFound($"No completed tasks found for user {userId}.");
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get completed tasks per month for user {UserId}", userId);
                return StatusCode(500, $"Failed to get completed tasks per month for user {userId}");
            }
        }

        /// <summary>
        /// Gets statistics for completed tasks per month for a user and their friends (line chart data).
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <returns>Line chart data for completed tasks per month by the user and their friends.</returns>
        /// <response code="200">Returns the combined completed tasks per month statistics.</response>
        /// <response code="400">If the user ID is missing.</response>
        /// <response code="404">If there are no completed tasks for the user and friends.</response>
        /// <response code="500">If an error occurs.</response>
        [HttpGet("user/{userId}/friends/completed/monthly")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserAndFriendsTasksCompletedPerMonth(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("User ID is required.");

            try
            {
                var stats = await _taskStatisticByDateService.GetUserAndFriendsTasksCompletedPerMonthAsync(userId);
                if (stats == null || !stats.Any())
                    return NotFound($"No completed tasks found for user {userId} and friends.");
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get completed tasks per month for user {UserId} and friends", userId);
                return StatusCode(500, $"Failed to get completed tasks per month for user {userId} and friends");
            }
        }

        /// <summary>
        /// Gets global statistics for completed tasks per month for all users (line chart data).
        /// </summary>
        /// <returns>Line chart data for completed tasks per month for all users.</returns>
        /// <response code="200">Returns the global completed tasks per month statistics.</response>
        /// <response code="500">If an error occurs.</response>
        [HttpGet("global/completed/monthly")]
        [AllowAnonymous]
        public async Task<IActionResult> GetGlobalTasksCompletedPerMonth()
        {
            try
            {
                var stats = await _taskStatisticByDateService.GetGlobalTasksCompletedPerMonthAsync();
                if (stats == null || !stats.Any())
                    return NotFound("No completed tasks found for any users.");
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get global completed tasks per month");
                return StatusCode(500, "Failed to get global completed tasks per month");
            }
        }
    }

}
