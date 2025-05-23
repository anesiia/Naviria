using NaviriaAPI.DTOs;
using NaviriaAPI.DTOs.Achievement;

namespace NaviriaAPI.IServices
{
    public interface IAchievementService
    {
        Task<IEnumerable<AchievementDto>> GetAllAsync();
        Task<AchievementDto?> GetByIdAsync(string id);
        Task<AchievementDto> CreateAsync(AchievementCreateDto achievementDto);
        Task<bool> UpdateAsync(string id, AchievementUpdateDto  achievementDto);
        Task<bool> DeleteAsync(string id);
        Task<IEnumerable<AchievementDto>> GetAllUserAchievementsAsync(string userId);
        Task<bool> AwardAchievementPointsAsync(string userId, string achievementId);
    }
}
