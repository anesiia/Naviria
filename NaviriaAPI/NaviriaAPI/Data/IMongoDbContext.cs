using MongoDB.Driver;
using NaviriaAPI.Entities;

namespace NaviriaAPI.Data
{
    public interface IMongoDbContext
    {
        IMongoDatabase GetDatabase();
        IMongoCollection<AchievementEntity> Achievements { get; }
        IMongoCollection<AssistantChatMessageEntity> AssistantChatMessages { get; }
        IMongoCollection<CategoryEntity> Categories { get; }
        IMongoCollection<FolderEntity> Folders { get; }
        IMongoCollection<FriendRequestEntity> FriendsRequests { get; }
        IMongoCollection<NotificationEntity> Notifications { get; }
        IMongoCollection<QuoteEntity> Quotes { get; }
        IMongoCollection<UserEntity> Users { get; }

    }
}
