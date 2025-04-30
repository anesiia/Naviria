using NaviriaAPI.Entities;

namespace NaviriaAPI.IRepositories
{
    public interface IAchievementRepository
    {
        Task<List<AchievementEntity>> GetAllAsync();
        Task<AchievementEntity?> GetByIdAsync(string id);
        Task CreateAsync(AchievementEntity category);
        Task<bool> UpdateAsync(AchievementEntity category);
        Task<bool> DeleteAsync(string id);
        Task<IEnumerable<AchievementEntity>> GetManyByIdsAsync(IEnumerable<string> ids);
    }
}