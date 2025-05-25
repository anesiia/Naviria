using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.IServices.IStatisticServices;

namespace NaviriaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaderboardController : ControllerBase
    {
        private readonly ILeaderboardService _leaderboardService;
        private readonly ILogger<LeaderboardController> _logger;

        public LeaderboardController(
            ILeaderboardService leaderboardService,
            ILogger<LeaderboardController> logger)
        {
            _leaderboardService = leaderboardService;
            _logger = logger;
        }

        /// <summary>
        /// Gets the top 10 users in the leaderboard.
        /// </summary>
        /// <returns>Leaderboard of users with their stats.</returns>
        /// <response code="200">Returns the leaderboard.</response>
        /// <response code="500">If an error occurs.</response>
        [HttpGet("top")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTopLeaderboardUsers()
        {
            try
            {
                var leaders = await _leaderboardService.GetTopLeaderboardUsersAsync();
                return Ok(leaders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get leaderboard.");
                return StatusCode(500, "Failed to get leaderboard.");
            }
        }
    }

}
