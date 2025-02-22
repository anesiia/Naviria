using MongoDB.Driver;
using NaviriaAPI.Entities;
using NaviriaAPI.Data;
using NaviriaAPI.IRepositories;

namespace NaviriaAPI.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IMongoCollection<CategoryEntity> _categories;

        public CategoryRepository(IMongoDatabase database)
        {
            _categories = database.GetCollection<CategoryEntity>("categories");
           // _categories = database.GetDatabase().GetCollection<CategoryEntity>("categories");
        }

        public async Task<List<CategoryEntity>> GetAllAsync() =>
            await _categories.Find(_ => true).ToListAsync();

        public async Task<CategoryEntity?> GetByIdAsync(string id) =>
            await _categories.Find(c => c.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(CategoryEntity category) =>
            await _categories.InsertOneAsync(category);

        public async Task<bool> UpdateAsync(CategoryEntity category)
        {
            var result = await _categories.ReplaceOneAsync(c => c.Id == category.Id, category);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _categories.DeleteOneAsync(c => c.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
