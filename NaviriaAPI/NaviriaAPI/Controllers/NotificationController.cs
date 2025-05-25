using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.DTOs.Notification;
using NaviriaAPI.IServices;

namespace NaviriaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(INotificationService notificationService, ILogger<NotificationController> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var notifications = await _notificationService.GetAllAsync();
            return Ok(notifications);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Notification ID is required.");

            var notification = await _notificationService.GetByIdAsync(id);
            return notification == null ? NotFound() : Ok(notification);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NotificationCreateDto notificationDto)
        {
            if (notificationDto == null)
                return BadRequest("Notification data is required.");

            try
            {
                var created = await _notificationService.CreateAsync(notificationDto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create notification.");
                return StatusCode(500, "An error occurred while creating the notification.");
            }
        }

        [HttpPut("user/{userId}/mark-all-read")]
        public async Task<IActionResult> MarkAllAsRead(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("User ID is required.");

            try
            {
                await _notificationService.MarkAllUserNotificationsAsReadAsync(userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update user notifications with ID: {NotificationId}", userId);
                return StatusCode(500, "Failed to update user notifications .");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Notification ID is required.");

            try
            {
                var deleted = await _notificationService.DeleteAsync(id);
                return deleted ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete notification with ID: {NotificationId}", id);
                return StatusCode(500, "Failed to delete notification.");
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllUserNotifications(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("User ID is required.");

            try
            {
                var notifications = await _notificationService.GetAllUserNotificationsAsync(userId);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get notifications for user {UserId}", userId);
                return StatusCode(500, "Failed to get user notifications.");
            }
        }
    }
}
