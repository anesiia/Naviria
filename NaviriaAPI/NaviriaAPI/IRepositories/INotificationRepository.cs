using NaviriaAPI.Entities;

namespace NaviriaAPI.IRepositories
{
    public interface INotificationRepository
    {
        Task<List<NotificationEntity>> GetAllAsync();
        Task<NotificationEntity?> GetByIdAsync(string id);
        Task CreateAsync(NotificationEntity notificationEntity);
        Task<bool> DeleteAsync(string id);
        Task<bool> UpdateStatusAsync(NotificationEntity notificationEntity);
        Task<List<NotificationEntity>> GetAllByUserAsync(string userId);
        
    }
}
