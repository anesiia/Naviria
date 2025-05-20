using NaviriaAPI.DTOs;

namespace NaviriaAPI.IServices
{
    public interface IUserSearchService
    {
        Task<List<UserDto>> GetUsersByTaskCategoryAsync(string categoryId);
        Task<IEnumerable<UserDto>> SearchPotentialFriendsByNicknameAsync(string userId, string query);
        Task<IEnumerable<UserDto>> SearchFriendsByNicknameAsync(string userId, string query);
        Task<IEnumerable<UserDto>> SearchIncomingFriendRequestsByNicknameAsync(string userId, string query);
    }
}
