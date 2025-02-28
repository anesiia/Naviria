using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.DTOs;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices;
using NaviriaAPI.Mappings;

namespace NaviriaAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<UserDto> CreateAsync(UserCreateDto createDto)
        {
            var entity = UserMapper.ToEntity(createDto);
            await _userRepository.CreateAsync(entity);
            return UserMapper.ToDto(entity);
        }
        public async Task<bool> UpdateAsync(string id, UserUpdateDto updateDto)
        {
            var entity = UserMapper.ToEntity(id, updateDto);
            return await _userRepository.UpdateAsync(entity);
        }
        public async Task<UserDto?> GetByIdAsync(string id)
        {
            var entity = await _userRepository.GetByIdAsync(id);
            return entity == null ? null : UserMapper.ToDto(entity);
        }

        public async Task<bool> DeleteAsync(string id) =>
            await _userRepository.DeleteAsync(id);

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var categories = await _userRepository.GetAllAsync();
            return categories.Select(UserMapper.ToDto).ToList();
        }
    }
}
