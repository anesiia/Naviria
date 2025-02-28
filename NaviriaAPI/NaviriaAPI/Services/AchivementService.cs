using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.IServices;
using NaviriaAPI.DTOs;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Mappings;
using NaviriaAPI.DTOs.UdateDTOs;

namespace NaviriaAPI.Services
{
    public class AchievementService : IAchievementService
    {
        private readonly IAchievementRepository _achievementRepository;
        public AchievementService(IAchievementRepository achievementRepository)
        {
            _achievementRepository = achievementRepository;
        }
        public async Task<AchievementDto> CreateAsync(AchievementCreateDto createDto)
        {
            var entity = AchievementMapper.ToEntity(createDto);
            await _achievementRepository.CreateAsync(entity);
            return AchievementMapper.ToDto(entity);
        }
        public async Task<bool> UpdateAsync(string id, AchievementUpdateDto updateDto)
        {
            var entity = AchievementMapper.ToEntity(id, updateDto);
            return await _achievementRepository.UpdateAsync(entity);
        }
        public async Task<AchievementDto?> GetByIdAsync(string id)
        {
            var entity = await _achievementRepository.GetByIdAsync(id);
            return entity == null ? null : AchievementMapper.ToDto(entity);
        }

        public async Task<bool> DeleteAsync(string id) =>
            await _achievementRepository.DeleteAsync(id);

        public async Task<IEnumerable<AchievementDto>> GetAllAsync()
        {
            var achievements = await _achievementRepository.GetAllAsync();
            return achievements.Select(AchievementMapper.ToDto).ToList();
        }
    }
}
