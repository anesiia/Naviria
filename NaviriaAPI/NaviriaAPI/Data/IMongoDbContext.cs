using MongoDB.Driver;
using NaviriaAPI.Entities;

namespace NaviriaAPI.Data
{
    public interface IMongoDbContext
    {
        IMongoDatabase GetDatabase();
        IMongoCollection<AchievementEntity> Achievements { get; }
        IMongoCollection<CategoryEntity> Categories { get; }
        IMongoCollection<FriendRequestEntity> FriendsRequests { get; }
        IMongoCollection<QuoteEntity> Quotes { get; }
        IMongoCollection<UserEntity> Users { get; }

    }

}
