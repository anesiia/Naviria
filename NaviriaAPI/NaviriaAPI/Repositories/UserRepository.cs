using MongoDB.Driver;
using NaviriaAPI.Data;
using NaviriaAPI.Entities;
using NaviriaAPI.IRepositories;

namespace NaviriaAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<UserEntity> _users;

        public UserRepository(IMongoDbContext database)
        {
            _users = database.Users;
        }

        public async Task<List<UserEntity>> GetAllAsync() =>
            await _users.Find(_ => true).ToListAsync();

        public async Task<UserEntity?> GetByIdAsync(string id) =>
            await _users.Find(c => c.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(UserEntity user) =>
            await _users.InsertOneAsync(user);

        public async Task<bool> UpdateAsync(UserEntity user)
        {
            var result = await _users.ReplaceOneAsync(c => c.Id == user.Id, user);
            return result.ModifiedCount > 0;
        }
        public async Task<UserEntity> GetByEmailAsync(string email) =>
            await _users.Find(u => u.Email == email).FirstOrDefaultAsync();

        public async Task<UserEntity> GetByNicknameAsync(string nickname) =>
            await _users.Find(u => u.Nickname == nickname).FirstOrDefaultAsync();

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _users.DeleteOneAsync(c => c.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task<bool> UpdatePresenceAsync(string id, DateTime dateTime, bool isOnline)
        {
            var filter = Builders<UserEntity>.Filter.Eq(u => u.Id, id);
            var update = Builders<UserEntity>.Update
                .Set(u => u.LastSeen, dateTime)
                .Set(u => u.IsOnline, isOnline);

            var result = await _users.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> UpdateProfileImageAsync(string userId, string imageUrl)
        {
            var filter = Builders<UserEntity>.Filter.Eq(u => u.Id, userId);
            var update = Builders<UserEntity>.Update.Set(u => u.Photo, imageUrl);

            var result = await _users.UpdateOneAsync(filter, update);

            return result.ModifiedCount > 0;
        }

        public async Task<List<UserEntity>> GetManyByIdsAsync(IEnumerable<string> ids)
        {
            var filter = Builders<UserEntity>.Filter.In(u => u.Id, ids);
            return await _users.Find(filter).ToListAsync();
        }

        public async Task RemoveAchievementFromAllUsersAsync(string achievementId)
        {
            var filter = Builders<UserEntity>.Filter.ElemMatch(
                u => u.Achievements, a => a.AchievementId == achievementId);

            var update = Builders<UserEntity>.Update.PullFilter(
                u => u.Achievements, a => a.AchievementId == achievementId);

            await _users.UpdateManyAsync(filter, update);
        }


    }
}
