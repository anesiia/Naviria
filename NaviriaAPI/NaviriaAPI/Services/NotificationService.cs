using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.DTOs;
using NaviriaAPI.Exceptions;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices;
using NaviriaAPI.Mappings;
using NaviriaAPI.Entities;
using Microsoft.Extensions.Logging;
using NaviriaAPI.Services.User;

namespace NaviriaAPI.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserService _userService;

        public NotificationService(
            ILogger<NotificationService> logger,
            INotificationRepository notificationRepository,
            IUserService userService)
        {
            _logger = logger;
            _notificationRepository = notificationRepository;
            _userService = userService;
        }

        public async Task<NotificationDto> CreateAsync(NotificationCreateDto notificationDto)
        {
            if (notificationDto == null)
            {
                _logger.LogWarning("Attempted to create a notification with null DTO.");
                throw new ArgumentNullException(nameof(notificationDto), "Notification DTO cannot be null.");
            }

            var entity = NotificationMapper.ToEntity(notificationDto);
            await _notificationRepository.CreateAsync(entity);

            return NotificationMapper.ToDto(entity);
        }

        public async Task<IEnumerable<NotificationDto>> GetAllAsync()
        {

            var notifications = await _notificationRepository.GetAllAsync();
            if (!notifications.Any())
            {
                _logger.LogInformation("No notifications found.");
            }

            return notifications.Select(NotificationMapper.ToDto).ToList();
        }

        public async Task<NotificationDto?> GetByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("GetByIdAsync was called with an empty or null ID.");
                throw new ArgumentException("Notification ID cannot be null or empty.", nameof(id));
            }

            var entity = await _notificationRepository.GetByIdAsync(id);
            if (entity == null)
            {
                _logger.LogWarning("Notification with ID {NotificationId} was not found.", id);
                return null;
            }

            return NotificationMapper.ToDto(entity);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("DeleteAsync was called with an empty or null ID.");
                throw new ArgumentException("Notification ID cannot be null or empty.", nameof(id));
            }

            var result = await _notificationRepository.DeleteAsync(id);
            if (result)
                _logger.LogInformation("Notification with ID {NotificationId} was successfully deleted.", id);
            else
                _logger.LogWarning("Failed to delete notification with ID: {NotificationId}.", id);

            return result;
        }

        public async Task<IEnumerable<NotificationDto>> GetAllUserNotificationsAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                _logger.LogWarning("GetAllUserNotificationsAsync was called with an empty or null userId.");
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));
            }

            if (!await _userService.UserExistsAsync(userId))
            {
                _logger.LogWarning("User with ID {UserId} not found.", userId);
                throw new NotFoundException($"User with ID {userId} does not exist.");
            }


            var entities = await _notificationRepository.GetAllByUserAsync(userId);
            if (!entities.Any())
            {
                _logger.LogInformation("No notifications found for user: {UserId}", userId);
            }

            return entities.Select(NotificationMapper.ToDto);
        }

        public async Task MarkAllUserNotificationsAsReadAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                _logger.LogWarning("MarkAllUserNotificationsAsReadAsync was called with a null or empty userId.");
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));
            }

            await _notificationRepository.MarkAllAsReadByUserAsync(userId);
        }


        public async Task<bool> UpdateStatusAsync(string id, NotificationUpdateDto updateNotificationDto)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("UpdateStatusAsync was called with an empty or null ID.");
                throw new ArgumentException("Notification ID cannot be null or empty.", nameof(id));
            }

            if (updateNotificationDto == null)
            {
                _logger.LogWarning("Attempted to update notification status with null DTO.");
                throw new ArgumentNullException(nameof(updateNotificationDto), "Update DTO cannot be null.");
            }

            var entity = await _notificationRepository.GetByIdAsync(id);
            if (entity == null)
            {
                _logger.LogError("Notification with ID {NotificationId} not found for update.", id);
                throw new NotFoundException("Notification not found.");
            }

            entity.IsNew = updateNotificationDto.IsNew;

            var updated = await _notificationRepository.UpdateStatusAsync(entity);
            if (!updated)
            {
                _logger.LogError("Failed to update status for notification with ID: {NotificationId}", id);
                throw new FailedToUpdateException("Failed to update notification status.");
            }

            return true;
        }
    }
}
