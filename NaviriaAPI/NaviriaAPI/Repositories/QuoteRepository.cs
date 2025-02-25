using MongoDB.Driver;
using NaviriaAPI.Data;
using NaviriaAPI.Entities;
using NaviriaAPI.IRepositories;

namespace NaviriaAPI.Repositories
{
    public class QuoteRepository : IQuoteRepository
    {
        private readonly IMongoCollection<QuoteEntity> _quotes;

        public QuoteRepository(IMongoDbContext database)
        {
            _quotes = database.Quotes;
        }

        public async Task<List<QuoteEntity>> GetAllAsync() =>
            await _quotes.Find(_ => true).ToListAsync();

        public async Task<QuoteEntity?> GetByIdAsync(string id) =>
            await _quotes.Find(c => c.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(QuoteEntity quote) =>
            await _quotes.InsertOneAsync(quote);

        public async Task<bool> UpdateAsync(QuoteEntity quote)
        {
            var result = await _quotes.ReplaceOneAsync(c => c.Id == quote.Id, quote);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _quotes.DeleteOneAsync(c => c.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
