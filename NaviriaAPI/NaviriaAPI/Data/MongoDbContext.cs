using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NaviriaAPI.Entities;
using NaviriaAPI.Options;

namespace NaviriaAPI.Data
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDbOptions> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoDatabase GetDatabase() => _database;
        public IMongoCollection<AchievementEntity> Achievements => _database.GetCollection<AchievementEntity>("achievements");
        public IMongoCollection<AssistantChatMessageEntity> AssistantChatMessages => _database.GetCollection<AssistantChatMessageEntity>("ai_chat_messages");
        public IMongoCollection<CategoryEntity> Categories => _database.GetCollection<CategoryEntity>("categories");
        public IMongoCollection<FolderEntity> Folders => _database.GetCollection<FolderEntity>("folders");
        public IMongoCollection<FriendRequestEntity> FriendsRequests => _database.GetCollection<FriendRequestEntity>("friends_requests");
        public IMongoCollection<NotificationEntity> Notifications => _database.GetCollection<NotificationEntity>("notifications");
        public IMongoCollection<QuoteEntity> Quotes => _database.GetCollection<QuoteEntity>("quotes");
        public IMongoCollection<UserEntity> Users => _database.GetCollection<UserEntity>("users");

    }
}
