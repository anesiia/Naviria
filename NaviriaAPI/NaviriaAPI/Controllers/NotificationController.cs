using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.DTOs.Notification;
using NaviriaAPI.IServices;

namespace NaviriaAPI.Controllers
{
    /// <summary>
    /// API controller for managing user notifications.
    /// Provides endpoints to create, retrieve, update, and delete notifications.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationController"/> class.
        /// </summary>
        /// <param name="notificationService">Service for notification operations.</param>
        /// <param name="logger">Logger instance.</param>
        public NotificationController(INotificationService notificationService, ILogger<NotificationController> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        /// <summary>
        /// Gets a list of all notifications in the system.
        /// </summary>
        /// <returns>
        /// 200: Returns a list of notifications.<br/>
        /// 500: If an internal error occurs.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var notifications = await _notificationService.GetAllAsync();
            return Ok(notifications);
        }

        /// <summary>
        /// Gets a notification by its identifier.
        /// </summary>
        /// <param name="id">The notification identifier.</param>
        /// <returns>
        /// 200: Returns the notification.<br/>
        /// 400: If the notification ID is missing.<br/>
        /// 404: If the notification is not found.
        /// </returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Notification ID is required.");

            var notification = await _notificationService.GetByIdAsync(id);
            return notification == null ? NotFound() : Ok(notification);
        }

        /// <summary>
        /// Creates a new notification.
        /// </summary>
        /// <param name="notificationDto">The notification creation DTO.</param>
        /// <returns>
        /// 201: The created notification with its ID.<br/>
        /// 400: If input data is invalid.<br/>
        /// 500: If an internal error occurs.
        /// </returns>
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

        /// <summary>
        /// Marks all notifications for a specific user as read.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>
        /// 204: If the update was successful.<br/>
        /// 400: If the user ID is missing.<br/>
        /// 500: If an internal error occurs.
        /// </returns>
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
                return StatusCode(500, "Failed to update user notifications.");
            }
        }

        /// <summary>
        /// Deletes a notification by its identifier.
        /// </summary>
        /// <param name="id">The notification identifier.</param>
        /// <returns>
        /// 204: If the deletion was successful.<br/>
        /// 400: If the notification ID is missing.<br/>
        /// 404: If the notification is not found.<br/>
        /// 500: If an internal error occurs.
        /// </returns>
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

        /// <summary>
        /// Gets all notifications for a specific user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>
        /// 200: Returns a list of notifications for the user.<br/>
        /// 400: If the user ID is missing.<br/>
        /// 500: If an internal error occurs.
        /// </returns>
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
