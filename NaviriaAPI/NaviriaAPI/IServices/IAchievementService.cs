using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.DTOs;

namespace NaviriaAPI.IServices
{
    public interface IAchievementService
    {
        Task<IEnumerable<AchievementDto>> GetAllAsync();
        Task<AchievementDto?> GetByIdAsync(string id);
        Task<AchievementDto> CreateAsync(AchievementCreateDto achievementDto);
        Task<bool> UpdateAsync(string id, AchievementUpdateDto  achievementDto);
        Task<bool> DeleteAsync(string id);
    }
}
