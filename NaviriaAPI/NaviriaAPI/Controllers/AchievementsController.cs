using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.IServices;
using NaviriaAPI.IServices.ICleanupServices;
using NaviriaAPI.DTOs.Achievement;
using Microsoft.AspNetCore.Authorization;

namespace NaviriaAPI.Controllers
{
    /// <summary>
    /// API controller for managing achievements.
    /// Provides endpoints to create, retrieve, update, and delete achievements,
    /// as well as assign achievements to users.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AchievementsController : ControllerBase
    {
        private readonly IAchievementService _achievementsService;
        private readonly IAchievementCleanupService _achievementCleanupService;
        private readonly ILogger<AchievementsController> _logger;

        public AchievementsController(IAchievementService achievementsService,
            IAchievementCleanupService achievementCleanupService,
            ILogger<AchievementsController> logger)
        {
            _achievementsService = achievementsService;
            _achievementCleanupService = achievementCleanupService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a list of all achievements.
        /// </summary>
        /// <returns>
        /// 200: Returns a list of achievements.<br/>
        /// 500: If an internal error occurs.
        /// </returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var achievements = await _achievementsService.GetAllAsync();
                return Ok(achievements);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get achievements");
                return StatusCode(500, "Failed to get achievements");
            }
        }

        /// <summary>
        /// Retrieves an achievement by its identifier.
        /// </summary>
        /// <param name="id">The achievement identifier.</param>
        /// <returns>
        /// 200: The requested achievement.<br/>
        /// 404: If the achievement is not found.<br/>
        /// 500: If an internal error occurs.
        /// </returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var achievement = await _achievementsService.GetByIdAsync(id);
                if (achievement == null) return NotFound();
                return Ok(achievement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get achievement with ID {0}", id);
                return StatusCode(500, $"Failed to get achievement with ID {id}");
            }
        }

        /// <summary>
        /// Creates a new achievement.
        /// </summary>
        /// <param name="achievementDto">The achievement creation DTO.</param>
        /// <returns>
        /// 201: The created achievement with its ID.<br/>
        /// 500: If an internal error occurs.
        /// </returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AchievementCreateDto achievementDto)
        {
            try
            {
                var createdAchievement = await _achievementsService.CreateAsync(achievementDto);
                return CreatedAtAction(nameof(GetById), new { id = createdAchievement.Id }, createdAchievement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add new achievement");
                return StatusCode(500, "Failed to add new achievement");
            }
        }

        /// <summary>
        /// Updates the details of an achievement by its identifier.
        /// </summary>
        /// <param name="id">The achievement identifier.</param>
        /// <param name="achievementDto">The updated achievement data.</param>
        /// <returns>
        /// 204: If the update was successful.<br/>
        /// 404: If the achievement is not found.<br/>
        /// 500: If an internal error occurs.
        /// </returns>
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] AchievementUpdateDto achievementDto)
        {
            try
            {
                var updated = await _achievementsService.UpdateAsync(id, achievementDto);
                return updated ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update achievement with ID {0}", id);
                return StatusCode(500, $"Failed to update achievement with ID {id}");
            }
        }

        /// <summary>
        /// Deletes an achievement and removes it from all users who had this achievement.
        /// </summary>
        /// <param name="id">The achievement identifier.</param>
        /// <returns>
        /// 204: If the deletion was successful.<br/>
        /// 404: If the achievement is not found.<br/>
        /// 500: If an internal error occurs.
        /// </returns>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var deleted = await _achievementCleanupService.DeleteAchievementAndRemoveFromUsersAsync(id);
                return deleted ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete achievement with ID {0}", id);
                return StatusCode(500, $"Failed to delete achievement with ID {id}");
            }
        }

        /// <summary>
        /// Retrieves all achievements for a specific user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>
        /// 200: A list of achievements for the user.<br/>
        /// 400: If the userId is missing.<br/>
        /// 500: If an internal error occurs.
        /// </returns>
        [Authorize]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllUserAchievements(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("User ID is required.");

            try
            {
                var notifications = await _achievementsService.GetAllUserAchievementsAsync(userId);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get achievements for user {UserId}", userId);
                return StatusCode(500, "Failed to get user achievements.");
            }
        }

        /// <summary>
        /// Awards achievement points to a specific user for a given achievement.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="achievementId">The achievement identifier.</param>
        /// <returns>
        /// 204: If points were awarded successfully.<br/>
        /// 400: If userId or achievementId is missing.<br/>
        /// 404: If the user or achievement is not found.<br/>
        /// 500: If an internal error occurs.
        /// </returns>
        [Authorize]
        [HttpPut("{userId}/award-achievement-points/{achievementId}")]
        public async Task<IActionResult> AwardAchievementPointsToUser(string userId, string achievementId)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(achievementId))
                return BadRequest("User ID and Achievement ID are required.");

            try
            {
                var updated = await _achievementsService.AwardAchievementPointsAsync(userId, achievementId);
                return updated ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to award achievements points to user with ID {0}", userId);
                return StatusCode(500, $"Failed to award achievements points to user with ID {userId}");
            }
        }
    }
}
