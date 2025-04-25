using NaviriaAPI.DTOs;
using NaviriaAPI.Mappings;
using NaviriaAPI.Services.User;

namespace NaviriaAPI.IServices
{
    public interface IFriendService
    {
        Task<IEnumerable<UserDto>> GetUserFriendsAsync(string userId);
        Task<bool> DeleteFriendAsync(string fromUserId, string friendId);
        Task<IEnumerable<UserDto>> GetPotentialFriendsAsync(string userId);
        Task<IEnumerable<UserDto>> SearchUsersByNicknameAsync(string userId, string query);

    }
}
