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
        public async Task<UserDto> CreateAsync(UserCreateDto newUser)
        {
            newUser.LastSeen = newUser.LastSeen.ToUniversalTime();
            var entity = UserMapper.ToEntity(newUser);
            await _userRepository.CreateAsync(entity);
            return UserMapper.ToDto(entity);
        }
        public async Task<bool> UpdateAsync(string id, UserUpdateDto updateUser)
        {
            updateUser.LastSeen = updateUser.LastSeen.ToUniversalTime();
            var entity = UserMapper.ToEntity(id, updateUser);
            return await _userRepository.UpdateAsync(entity);
        }
        public async Task<UserDto?> GetByIdAsync(string id)
        {
            var entity = await _userRepository.GetByIdAsync(id);
            entity.LastSeen = entity.LastSeen.ToLocalTime();
            return entity == null ? null : UserMapper.ToDto(entity);
        }

        public async Task<bool> DeleteAsync(string id) =>
            await _userRepository.DeleteAsync(id);

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            users.ForEach(user => user.LastSeen = user.LastSeen.ToLocalTime());
            return users.Select(UserMapper.ToDto).ToList();
        }
    }
}
