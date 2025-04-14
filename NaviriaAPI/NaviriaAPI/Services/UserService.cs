using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.DTOs;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices;
using NaviriaAPI.Mappings;
using Microsoft.AspNetCore.Identity;
using NaviriaAPI.Entities;
using OpenAI.Chat;
using NaviriaAPI.Services.Validation;
using NaviriaAPI.IServices.ICloudStorage;

namespace NaviriaAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<UserEntity> _passwordHasher;
        private readonly UserValidationService _validation;
        private readonly string _openAIKey;
        public readonly ICloudinaryService _cloudinaryService;
        public UserService(
            IUserRepository userRepository, 
            IPasswordHasher<UserEntity> passwordHasher,
            IConfiguration config,
            UserValidationService validation,
            ICloudinaryService cloudinaryService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _validation = validation;
            _openAIKey = config["OpenAIKey"]
                ?? throw new InvalidOperationException("OpenAIKey is missing in configuration.");
            _cloudinaryService = cloudinaryService;
        }
        public async Task<UserDto> CreateAsync(UserCreateDto userDto)
        {
            await _validation.ValidateAsync(userDto);

            userDto.LastSeen = userDto.LastSeen.ToUniversalTime();
            var entity = UserMapper.ToEntity(userDto);
            entity.Password = _passwordHasher.HashPassword(entity, userDto.Password);
            await _userRepository.CreateAsync(entity);

            return UserMapper.ToDto(entity);
        }
        public async Task<bool> UpdateAsync(string id, UserUpdateDto userDto)
        {
            UserValidationService.ValidateAsync(userDto);

            var existing = await _userRepository.GetByIdAsync(id);
            if (existing == null)
                return false;

            userDto.LastSeen = userDto.LastSeen.ToUniversalTime();
            var entity = UserMapper.ToEntity(id, userDto);
            return await _userRepository.UpdateAsync(entity);
        }
        public async Task<UserDto?> GetByIdAsync(string id)
        {
            var entity = await _userRepository.GetByIdAsync(id);
            if (entity == null)
                return null;

            entity.LastSeen = entity.LastSeen.ToLocalTime();
            return UserMapper.ToDto(entity);
        }

        public async Task<bool> DeleteAsync(string id) =>
            await _userRepository.DeleteAsync(id);

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            users.ForEach(user => user.LastSeen = user.LastSeen.ToLocalTime());
            return users.Select(UserMapper.ToDto).ToList();
        }
        public async Task<string> GetAiAnswerAsync(string question)
        {
            var modelName = "gpt-4o-mini";
            var client = new ChatClient(modelName, _openAIKey);

            var responce = await client.CompleteChatAsync(question);

            return responce.Value.Content[0].Text;
        }
    }
}
