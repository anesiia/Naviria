using MongoDB.Driver;
using NaviriaAPI.Entities;
using NaviriaAPI.Helpers;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Data;

namespace NaviriaAPI.Repositories
{
    public class StatisticRepository : IStatisticRepository
    {
        private readonly IMongoCollection<UserEntity> _users;
        private readonly IMongoCollection<TaskEntity> _tasks;

        public StatisticRepository(IMongoDbContext database)
        {
            _users = database.Users;
            _tasks = database.Tasks;
        }

        public async Task<int> GetTotalUserCountAsync()
        {
            return (int)await _users.CountDocumentsAsync(_ => true);
        }

        public async Task<int> GetTotalTaskCountAsync()
        {
            return (int)await _tasks.CountDocumentsAsync(_ => true);
        }

        public async Task<int> GetCompletedTaskCountAsync()
        {
            var filter = Builders<TaskEntity>.Filter.In(t => t.Status, new[] {
                CurrentTaskStatus.Completed,
                CurrentTaskStatus.CompletedInTime,
                CurrentTaskStatus.CompletedNotInTime
            });
            return (int)await _tasks.CountDocumentsAsync(filter);
        }

        public async Task<DateTime?> GetUserRegistrationDateAsync(string userId)
        {
            var user = await _users.Find(u => u.Id == userId).FirstOrDefaultAsync();
            return user?.RegitseredAt;
        }
    }
}
