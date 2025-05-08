using MongoDB.Driver;
using NaviriaAPI.Data;
using NaviriaAPI.Entities;
using NaviriaAPI.IRepositories;

namespace NaviriaAPI.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly IMongoCollection<TaskEntity> _tasks;

        public TaskRepository(IMongoDbContext dbContext)
        {
            _tasks = dbContext.GetDatabase().GetCollection<TaskEntity>("tasks");
        }

        public async Task<TaskEntity?> GetByIdAsync(string id)
        {
            return await _tasks.Find(t => t.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TaskEntity>> GetAllByUserAsync(string userId)
        {
            return await _tasks.Find(t => t.UserId == userId).ToListAsync();
        }

        public async Task CreateAsync(TaskEntity entity)
        {
            await _tasks.InsertOneAsync(entity);
        }

        public async Task<bool> UpdateAsync(TaskEntity entity)
        {
            var result = await _tasks.ReplaceOneAsync(t => t.Id == entity.Id, entity);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _tasks.DeleteOneAsync(t => t.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task DeleteManyByUserIdAsync(string userId)
        {
            var filter = Builders<TaskEntity>.Filter.Eq(t => t.UserId, userId);
            await _tasks.DeleteManyAsync(filter);
        }
    }
}