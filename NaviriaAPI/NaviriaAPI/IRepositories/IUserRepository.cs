using NaviriaAPI.Entities;

namespace NaviriaAPI.IRepositories
{
    public interface IUserRepository
    {
        Task<List<UserEntity>> GetAllAsync();
        Task<UserEntity?> GetByIdAsync(string id);
        Task CreateAsync(UserEntity quote);
        Task<bool> UpdateAsync(UserEntity quote);
        Task<bool> DeleteAsync(string id);
    }
}
