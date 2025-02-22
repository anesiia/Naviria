using NaviriaAPI.Entities;

namespace NaviriaAPI.IRepositories
{
    public interface IQuoteRepository
    {
        Task<List<QuoteEntity>> GetAllAsync();
        Task<QuoteEntity?> GetByIdAsync(string id);
        Task CreateAsync(QuoteEntity quote);
        Task<bool> UpdateAsync(QuoteEntity quote);
        Task<bool> DeleteAsync(string id);
    }
}
