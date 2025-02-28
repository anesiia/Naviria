using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.DTOs;

namespace NaviriaAPI.IServices
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(string id);
        Task<UserDto> CreateAsync(UserCreateDto userDto);
        Task<bool> UpdateAsync(string id, UserUpdateDto userDto);
        Task<bool> DeleteAsync(string id);
    }
}
