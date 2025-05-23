using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Mappings;
using NaviriaAPI.IServices;
using NaviriaAPI.DTOs.Quote;

namespace NaviriaAPI.Services
{
    public class QuoteService : IQuoteService
    {
        private readonly IQuoteRepository _quoteRepository;
        public QuoteService(IQuoteRepository quoteRepository)
        {
            _quoteRepository = quoteRepository;
        }
        public async Task<QuoteDto> CreateAsync(QuoteCreateDto quoteDto)
        {
            var entity = QuoteMapper.ToEntity(quoteDto);
            await _quoteRepository.CreateAsync(entity);
            return QuoteMapper.ToDto(entity);
        }
        public async Task<bool> UpdateAsync(string id, QuoteUpdateDto quoteDto)
        {
            var entity = QuoteMapper.ToEntity(id, quoteDto);
            return await _quoteRepository.UpdateAsync(entity);
        }
        public async Task<QuoteDto?> GetByIdAsync(string id)
        {
            var entity = await _quoteRepository.GetByIdAsync(id);
            return entity == null ? null : QuoteMapper.ToDto(entity);
        }

        public async Task<bool> DeleteAsync(string id) =>
            await _quoteRepository.DeleteAsync(id);

        public async Task<IEnumerable<QuoteDto>> GetAllAsync()
        {
            var categories = await _quoteRepository.GetAllAsync();
            return categories.Select(QuoteMapper.ToDto).ToList();
        }
    }
}
