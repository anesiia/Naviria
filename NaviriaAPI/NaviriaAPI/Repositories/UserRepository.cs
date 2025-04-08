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
        
        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _users.DeleteOneAsync(c => c.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
