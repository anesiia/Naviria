using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UdateDTOs;
using NaviriaAPI.DTOs;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Mappings;
using NaviriaAPI.IServices;

namespace NaviriaAPI.Services
{
    public class QuoteService : IQuoteService
    {
        private readonly IQuoteRepository _quoteRepository;
        public QuoteService(IQuoteRepository quoteRepository)
        {
            _quoteRepository = quoteRepository;
        }
        public async Task<QuoteDto> CreateAsync(QuoteCreateDto createDto)
        {
            var entity = QuoteMapper.ToEntity(createDto);
            await _quoteRepository.CreateAsync(entity);
            return QuoteMapper.ToDto(entity);
        }
        public async Task<bool> UpdateAsync(string id, QuoteUpdateDto updateDto)
        {
            var entity = QuoteMapper.ToEntity(id, updateDto);
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
