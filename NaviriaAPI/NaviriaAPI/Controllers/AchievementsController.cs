using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.IServices;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.Services;

namespace NaviriaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AchievementsController : ControllerBase
    {
        private readonly IAchievementService _achievementsService;
        private readonly ILogger<AchievementsController> _logger;

        public AchievementsController(IAchievementService achievementsService, ILogger<AchievementsController> logger)
        {
            _achievementsService = achievementsService;
            _logger = logger;
        }

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var deleted = await _achievementsService.DeleteAsync(id);
                return deleted ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete achievement with ID {0}", id);
                return StatusCode(500, $"Failed to delete achievement with ID {id}");
            }
        }

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
