using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.DTOs;
using NaviriaAPI.Entities;

namespace NaviriaAPI.IServices
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(string id);
        Task<string> CreateAsync(UserCreateDto userDto);
        Task<bool> UpdateAsync(string id, UserUpdateDto userDto);
        Task<bool> DeleteAsync(string id);
        Task<bool> GiveAchievementAsync(string userId, string achievementId);
        Task<UserEntity> GetUserOrThrowAsync(string id);
        Task<bool> UserExistsAsync(string userId);
    }
}
