using NaviriaAPI.Entities;
using System.Collections.Specialized;

namespace NaviriaAPI.IRepositories
{
    public interface IUserRepository
    {
        Task<List<UserEntity>> GetAllAsync();
        Task<UserEntity?> GetByIdAsync(string id);
        Task CreateAsync(UserEntity quote);
        Task<bool> UpdateAsync(UserEntity quote);
        Task<UserEntity> GetByEmailAsync(string email);
        Task<bool> DeleteAsync(string id);
        Task<bool> UpdatePresenceAsync(string id, DateTime dateTime, bool isOnline);
    }
}
