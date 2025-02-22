using NaviriaAPI.Entities;

namespace NaviriaAPI.IRepositories
{
    public interface IAchivementRepository
    {
        Task<List<AchivementEntity>> GetAllAsync();
        Task<AchivementEntity?> GetByIdAsync(string id);
        Task CreateAsync(AchivementEntity category);
        Task<bool> UpdateAsync(AchivementEntity category);
        Task<bool> DeleteAsync(string id);
    }
}