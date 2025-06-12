using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.IServices.IStatisticServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace NaviriaAPI.Controllers
{
    /// <summary>
    /// Controller for system-wide statistics and analytics.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticGeneralController : ControllerBase
    {
        private readonly IGeneralStatisticService _statisticsService;
        private readonly ILogger<StatisticGeneralController> _logger;

        public StatisticGeneralController(
            IGeneralStatisticService statisticsService,
            ILogger<StatisticGeneralController> logger)
        {
            _statisticsService = statisticsService;
            _logger = logger;
        }

        /// <summary>
        /// Gets the total number of users in the system.
        /// </summary>
        /// <returns>Total user count.</returns>
        /// <response code="200">Returns the total user count.</response>
        /// <response code="500">If an error occurs.</response>
        [Authorize]
        [HttpGet("users/count")]
        public async Task<IActionResult> GetTotalUserCount()
        {
            try
            {
                var count = await _statisticsService.GetTotalUserCountAsync();
                return Ok(new { totalUsers = count });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get total user count.");
                return StatusCode(500, "Failed to get total user count.");
            }
        }

        /// <summary>
        /// Gets the total number of tasks in the system.
        /// </summary>
        /// <returns>Total task count.</returns>
        /// <response code="200">Returns the total task count.</response>
        /// <response code="500">If an error occurs.</response>
        [Authorize]
        [HttpGet("tasks/count")]
        public async Task<IActionResult> GetTotalTaskCount()
        {
            try
            {
                var count = await _statisticsService.GetTotalTaskCountAsync();
                return Ok(new { totalTasks = count });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get total task count.");
                return StatusCode(500, "Failed to get total task count.");
            }
        }

        /// <summary>
        /// Gets the percentage of tasks that are completed.
        /// </summary>
        /// <returns>Percentage of completed tasks (0..100).</returns>
        /// <response code="200">Returns the percentage of completed tasks.</response>
        /// <response code="500">If an error occurs.</response>
        [Authorize]
        [HttpGet("tasks/completed-percentage")]
        public async Task<IActionResult> GetCompletedTasksPercentage()
        {
            try
            {
                var percent = await _statisticsService.GetCompletedTasksPercentageAsync();
                return Ok(new { completedPercentage = percent });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get completed tasks percentage.");
                return StatusCode(500, "Failed to get completed tasks percentage.");
            }
        }

        /// <summary>
        /// Gets the number of days since the specified user's registration.
        /// </summary>
        /// <param name="userId">User's ID.</param>
        /// <returns>Days since registration.</returns>
        /// <response code="200">Returns number of days since registration.</response>
        /// <response code="400">If userId is missing.</response>
        /// <response code="404">If the user is not found.</response>
        /// <response code="500">If an error occurs.</response>
        [Authorize]
        [HttpGet("users/{userId}/days-since-registration")]
        public async Task<IActionResult> GetDaysSinceUserRegistration(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("User ID is required.");

            try
            {
                var days = await _statisticsService.GetDaysSinceUserRegistrationAsync(userId);
                if (days < 0)
                    return NotFound($"User with ID {userId} not found.");
                return Ok(new {daysSinceRegistration = days });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get days since registration for user with ID {UserId}", userId);
                return StatusCode(500, $"Failed to get days since registration for user with ID {userId}");
            }
        }

        /// <summary>
        /// Gets the number of days since the application's birthday (22.02.2025).
        /// </summary>
        /// <returns>Days since the app's birthday.</returns>
        /// <response code="200">Returns the number of days since the app's birthday.</response>
        /// <response code="500">If an error occurs.</response>
        [Authorize]
        [HttpGet("days-since-birthday")]
        public IActionResult GetDaysSinceAppBirthday()
        {
            try
            {
                var days = _statisticsService.GetDaysSinceAppBirthday();
                return Ok(new { daysSinceBirthday = days });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get days since app birthday.");
                return StatusCode(500, "Failed to get days since app birthday.");
            }
        }
    }
}
