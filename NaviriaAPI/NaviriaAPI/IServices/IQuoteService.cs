using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.Quote;

namespace NaviriaAPI.IServices
{
    public interface IQuoteService
    {
        Task<IEnumerable<QuoteDto>> GetAllAsync();
        Task<QuoteDto?> GetByIdAsync(string id);
        Task<QuoteDto> CreateAsync(QuoteCreateDto quoteDto);
        Task<bool> UpdateAsync(string id, QuoteUpdateDto quoteDto);
        Task<bool> DeleteAsync(string id);
    }
}
