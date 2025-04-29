using MongoDB.Driver;
using NaviriaAPI.Entities;
using NaviriaAPI.Data;
using NaviriaAPI.IRepositories;


namespace NaviriaAPI.Repositories
{
    public class AchievementRepository : IAchievementRepository
    {
        private readonly IMongoCollection<AchievementEntity> _achievements;

        public AchievementRepository(IMongoDbContext database)
        {
            _achievements = database.Achievements;
        }

        public async Task<List<AchievementEntity>> GetAllAsync() =>
            await _achievements.Find(_ => true).ToListAsync();

        public async Task<AchievementEntity?> GetByIdAsync(string id) =>
            await _achievements.Find(c => c.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(AchievementEntity achievement) =>
            await _achievements.InsertOneAsync(achievement);

        public async Task<bool> UpdateAsync(AchievementEntity achievement)
        {
            var result = await _achievements.ReplaceOneAsync(c => c.Id == achievement.Id, achievement);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _achievements.DeleteOneAsync(c => c.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task<IEnumerable<AchievementEntity>> GetManyByIdsAsync(IEnumerable<string> ids)
        {
            var filter = Builders<AchievementEntity>.Filter.In(a => a.Id, ids);
            return await _achievements.Find(filter).ToListAsync();
        }

    }
}
