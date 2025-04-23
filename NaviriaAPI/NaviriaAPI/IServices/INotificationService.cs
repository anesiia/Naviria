using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.DTOs;
using NaviriaAPI.Entities;

namespace NaviriaAPI.IServices
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDto>> GetAllAsync();
        Task<NotificationDto?> GetByIdAsync(string id);
        Task<NotificationDto> CreateAsync(NotificationCreateDto notificationDto);
        Task<bool> DeleteAsync(string id);
        Task<IEnumerable<NotificationDto>> GetAllUserNotificationsAsync(string userId);
        Task<bool> UpdateStatusAsync(string id, NotificationUpdateDto updateNotificationDto);
    }
}
