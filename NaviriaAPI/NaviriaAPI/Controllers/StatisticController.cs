using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.IServices.IStatisticServices;
using Microsoft.AspNetCore.Authorization;

namespace NaviriaAPI.Controllers
{
    /// <summary>
    /// Controller for statistics and analytics.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticController : ControllerBase
    {
        private readonly ITaskStatisticService _taskStatisticsService;
        private readonly ILogger<StatisticController> _logger;

        public StatisticController(
            ITaskStatisticService taskStatisticsService,
            ILogger<StatisticController> logger)
        {
            _taskStatisticsService = taskStatisticsService;
            _logger = logger;
        }

        /// <summary>
        /// Gets the total number of checked-in days for all repeatable subtasks of a user.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <returns>The total checked-in days count.</returns>
        /// <response code="200">Returns the total checked-in days count.</response>
        /// <response code="404">If the user has no tasks or repeatable subtasks.</response>
        /// <response code="500">If an error occurs.</response>
        [HttpGet("checkedin/total/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTotalCheckedInDaysForUser(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("User ID is required.");

            try
            {
                var total = await _taskStatisticsService.GetTotalCheckedInDaysCountForUserAsync(userId);
                return Ok(new { userId, totalCheckedInDays = total });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get total checked-in days for user with ID {UserId}", userId);
                return StatusCode(500, $"Failed to get statistics for user with ID {userId}");
            }
        }

        /// <summary>
        /// Gets the number of checked-in days for a specific repeatable subtask of a user.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <param name="subtaskId">The subtask ID.</param>
        /// <returns>The checked-in days count for the specified subtask.</returns>
        /// <response code="200">Returns the checked-in days count for the subtask.</response>
        /// <response code="404">If the subtask is not found or is not repeatable.</response>
        /// <response code="500">If an error occurs.</response>
        [HttpGet("checkedin/{userId}/{subtaskId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCheckedInDaysForSubtask(string userId, string subtaskId)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(subtaskId))
                return BadRequest("User ID and subtask ID are required.");

            try
            {
                var count = await _taskStatisticsService.GetCheckedInDaysCountForSubtaskAsync(userId, subtaskId);
                return Ok(new { userId, subtaskId, checkedInDays = count });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get checked-in days for subtask {SubtaskId} of user {UserId}", subtaskId, userId);
                return StatusCode(500, $"Failed to get statistics for user {userId} and subtask {subtaskId}");
            }
        }
    }
}
