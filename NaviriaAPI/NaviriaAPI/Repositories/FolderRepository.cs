// Repositories/FolderRepository.cs
using MongoDB.Driver;
using NaviriaAPI.Data;
using NaviriaAPI.Entities;
using NaviriaAPI.IRepositories;

namespace NaviriaAPI.Repositories
{
    public class FolderRepository : IFolderRepository
    {
        private readonly IMongoCollection<FolderEntity> _folders;

        public FolderRepository(IMongoDbContext context)
        {
            _folders = context.Folders;
        }

        public async Task<IEnumerable<FolderEntity>> GetAllByUserIdAsync(string userId)
        {
            return await _folders
                .Find(f => f.UserId == userId)
                .SortByDescending(f => f.CreatedAt)
                .ToListAsync();
        }

        public async Task<FolderEntity?> GetByIdAsync(string id)
        {
            return await _folders.Find(f => f.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(FolderEntity folder)
        {
            await _folders.InsertOneAsync(folder);
        }

        public async Task<bool> UpdateAsync(FolderEntity folder)
        {
            var result = await _folders.ReplaceOneAsync(f => f.Id == folder.Id, folder);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _folders.DeleteOneAsync(f => f.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task DeleteManyByUserIdAsync(string userId)
        {
            var filter = Builders<FolderEntity>.Filter.Eq(f => f.UserId, userId);
            await _folders.DeleteManyAsync(filter);
        }

    }
}