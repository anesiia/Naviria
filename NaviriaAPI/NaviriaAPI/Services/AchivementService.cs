using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.IServices;
using NaviriaAPI.DTOs;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Mappings;
using NaviriaAPI.DTOs.UdateDTOs;

namespace NaviriaAPI.Services
{
    public class AchivementService : IAchivementService
    {
        private readonly IAchivementRepository _achivementRepository;
        public AchivementService(IAchivementRepository achivementRepository)
        {
            _achivementRepository = achivementRepository;
        }
        public async Task<AchivementDto> CreateAsync(AchivementCreateDto createDto)
        {
            var entity = AchivementMapper.ToEntity(createDto);
            await _achivementRepository.CreateAsync(entity);
            return AchivementMapper.ToDto(entity);
        }
        public async Task<bool> UpdateAsync(string id, AchivementUpdateDto updateDto)
        {
            var entity = AchivementMapper.ToEntity(id, updateDto);
            return await _achivementRepository.UpdateAsync(entity);
        }
        public async Task<AchivementDto?> GetByIdAsync(string id)
        {
            var entity = await _achivementRepository.GetByIdAsync(id);
            return entity == null ? null : AchivementMapper.ToDto(entity);
        }

        public async Task<bool> DeleteAsync(string id) =>
            await _achivementRepository.DeleteAsync(id);

        public async Task<IEnumerable<AchivementDto>> GetAllAsync()
        {
            var categories = await _achivementRepository.GetAllAsync();
            return categories.Select(AchivementMapper.ToDto).ToList();
        }
    }
}
