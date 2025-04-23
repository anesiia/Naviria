using MongoDB.Driver;
using NaviriaAPI.Data;
using NaviriaAPI.Entities;
using NaviriaAPI.IRepositories;

namespace NaviriaAPI.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly IMongoCollection<NotificationEntity> _notifications;

        public NotificationRepository(IMongoDbContext database)
        {
            _notifications = database.Notifications;
        }

        public async Task<List<NotificationEntity>> GetAllAsync() =>
            await _notifications.Find(_ => true).ToListAsync();

        public async Task<NotificationEntity?> GetByIdAsync(string id) =>
            await _notifications.Find(c => c.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(NotificationEntity notificationEntity) =>
            await _notifications.InsertOneAsync(notificationEntity);

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _notifications.DeleteOneAsync(c => c.Id == id);
            return result.DeletedCount > 0;
        }
        public async Task<List<NotificationEntity>> GetAllByUserAsync(string userId) =>
            await _notifications.Find(n => n.UserId == userId).ToListAsync();

        public async Task<bool> UpdateStatusAsync(NotificationEntity notificationEntity)
        {
            var result = await _notifications.ReplaceOneAsync(c => c.Id == notificationEntity.Id, notificationEntity);
            return result.ModifiedCount > 0;
        }
    }
}
