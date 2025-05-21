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
        Task<UserEntity> GetByNicknameAsync(string nickname);
        Task<bool> DeleteAsync(string id);
        Task<bool> UpdatePresenceAsync(string id, DateTime dateTime, bool isOnline);
        Task<bool> UpdateProfileImageAsync(string userId, string imageUrl);
        Task<List<UserEntity>> GetManyByIdsAsync(IEnumerable<string> ids);
        /// <summary>
        /// Removes a specific achievement from all users' achievement lists.
        /// </summary>
        /// <param name="achievementId">The identifier of the achievement to remove.</param>
        Task RemoveAchievementFromAllUsersAsync(string achievementId);


    }
}
