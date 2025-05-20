using NaviriaAPI.DTOs;

namespace NaviriaAPI.IServices
{
    public interface IUserSearchService
    {
        Task<List<UserDto>> GetUsersByTaskCategoryAsync(string categoryId);
    }
}
