using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.DTOs;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices;
using NaviriaAPI.Mappings;
using Microsoft.AspNetCore.Identity;
using NaviriaAPI.Entities;
using NaviriaAPI.Services.JwtTokenService;
using Microsoft.VisualBasic;
using OpenAI.Chat;
using NaviriaAPI.DTOs.FeaturesDTOs;

namespace NaviriaAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<UserEntity> _passwordHasher;
        private readonly JwtService _jwtService;
        private readonly string _openAIKey;
        public UserService(
            IUserRepository userRepository, 
            IPasswordHasher<UserEntity> passwordHasher,
            IConfiguration config,
            string openAIKey)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtService = new JwtService(config);
            _openAIKey = openAIKey;
        }
        public async Task<UserDto> CreateAsync(UserCreateDto newUserDto)
        {
            //newUserDto.LastSeen = newUserDto.LastSeen.ToUniversalTime();
            var entity = UserMapper.ToEntity(newUserDto);
            entity.Password = _passwordHasher.HashPassword(entity, newUserDto.Password);
            await _userRepository.CreateAsync(entity);
            return UserMapper.ToDto(entity);
        }
        public async Task<bool> UpdateAsync(string id, UserUpdateDto newUserDto)
        {
            //newUserDto.LastSeen = newUserDto.LastSeen.ToUniversalTime();
            var entity = UserMapper.ToEntity(id, newUserDto);
            return await _userRepository.UpdateAsync(entity);
        }
        public async Task<UserDto?> GetByIdAsync(string id)
        {
            var entity = await _userRepository.GetByIdAsync(id);
            //entity.LastSeen = entity.LastSeen.ToLocalTime();
            return entity == null ? null : UserMapper.ToDto(entity);
        }

        public async Task<bool> DeleteAsync(string id) =>
            await _userRepository.DeleteAsync(id);

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            //users.ForEach(user => user.LastSeen = user.LastSeen.ToLocalTime());
            return users.Select(UserMapper.ToDto).ToList();
        }

        public async Task<string> LoginAsync(UserLoginDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (user == null)
                throw new ArgumentException("User with such email does not exist");

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user,
                user.Password, loginDto.Password);

            if (passwordVerificationResult != PasswordVerificationResult.Success)
                throw new UnauthorizedAccessException("Invalid email or password");

            return _jwtService.GenerateUserToken(user);
        }

        public async Task<string> GetAiAnswerAsync(string question)
        {
            var modelName = "gpt-4o-mini";
            var client = new ChatClient(modelName, _openAIKey);

            var responce = client.CompleteChat(question);

            return responce.Value.Content[0].Text;
        }
    }
}
