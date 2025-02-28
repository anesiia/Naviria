using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.DTOs;

namespace NaviriaAPI.IServices
{
    public interface IAchievementService
    {
        Task<IEnumerable<AchievementDto>> GetAllAsync();
        Task<AchievementDto?> GetByIdAsync(string id);
        Task<AchievementDto> CreateAsync(AchievementCreateDto AchievementDto);
        Task<bool> UpdateAsync(string id, AchievementUpdateDto  AchievementDto);
        Task<bool> DeleteAsync(string id);
    }
}
