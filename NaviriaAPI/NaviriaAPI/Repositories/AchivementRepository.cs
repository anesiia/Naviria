using MongoDB.Driver;
using NaviriaAPI.Entities;
using NaviriaAPI.Data;
using NaviriaAPI.IRepositories;


namespace NaviriaAPI.Repositories
{
    public class AchivementRepository : IAchivementRepository
    {
        private readonly IMongoCollection<AchivementEntity> _achivements;

        public AchivementRepository(IMongoDbContext database)
        {
            _achivements = database.Achivements;
        }

        public async Task<List<AchivementEntity>> GetAllAsync() =>
            await _achivements.Find(_ => true).ToListAsync();

        public async Task<AchivementEntity?> GetByIdAsync(string id) =>
            await _achivements.Find(c => c.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(AchivementEntity achivement) =>
            await _achivements.InsertOneAsync(achivement);

        public async Task<bool> UpdateAsync(AchivementEntity achivement)
        {
            var result = await _achivements.ReplaceOneAsync(c => c.Id == achivement.Id, achivement);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _achivements.DeleteOneAsync(c => c.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
