using MongoDB.Driver;
using NaviriaAPI.Entities;
using NaviriaAPI.Data;
using NaviriaAPI.IRepositories;


namespace NaviriaAPI.Repositories
{
    public class AchievementRepository : IAchievementRepository
    {
        private readonly IMongoCollection<AchievementEntity> _Achievements;

        public AchievementRepository(IMongoDbContext database)
        {
            _Achievements = database.Achievements;
        }

        public async Task<List<AchievementEntity>> GetAllAsync() =>
            await _Achievements.Find(_ => true).ToListAsync();

        public async Task<AchievementEntity?> GetByIdAsync(string id) =>
            await _Achievements.Find(c => c.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(AchievementEntity achievement) =>
            await _Achievements.InsertOneAsync(achievement);

        public async Task<bool> UpdateAsync(AchievementEntity achievement)
        {
            var result = await _Achievements.ReplaceOneAsync(c => c.Id == achievement.Id, achievement);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _Achievements.DeleteOneAsync(c => c.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
