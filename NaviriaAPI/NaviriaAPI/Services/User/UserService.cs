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
using NaviriaAPI.Entities.EmbeddedEntities;
using NaviriaAPI.IServices.IGamificationLogic;
using NaviriaAPI.Exceptions;
using NaviriaAPI.IServices.IJwtService;

namespace NaviriaAPI.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<UserEntity> _passwordHasher;
        private readonly UserValidationService _validation;
        private readonly IAchievementRepository _achievementRepository;
        private readonly ILevelService _levelService;
        private readonly IJwtService _jwtService;

        public UserService(
            IUserRepository userRepository,
            IPasswordHasher<UserEntity> passwordHasher,
            IConfiguration config,
            UserValidationService validation,
            IAchievementRepository achievementRepository,
            ILevelService levelService,
            IJwtService jwtService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _validation = validation;
            _achievementRepository = achievementRepository;
            _levelService = levelService;
            _jwtService = jwtService;
        }

        public async Task<string> CreateAsync(UserCreateDto userDto)
        {
            await _validation.ValidateAsync(userDto);

            var entity = UserMapper.ToEntity(userDto);
            entity.Password = _passwordHasher.HashPassword(entity, userDto.Password);

            await _userRepository.CreateAsync(entity);

            return _jwtService.GenerateUserToken(entity);
        }

        public async Task<bool> UpdateAsync(string id, UserUpdateDto userDto)
        {
            UserValidationService.ValidateAsync(userDto);

            var existing = await GetUserOrThrowAsync(id);
            userDto.LastSeen = userDto.LastSeen.ToUniversalTime();

            var entity = UserMapper.ToEntity(id, userDto);

            if (existing.Points != entity.Points)
                entity.LevelInfo = _levelService.CalculateLevelProgress(entity.Points);
            else
                entity.LevelInfo = existing.LevelInfo;

            return await _userRepository.UpdateAsync(entity);
        }

        public async Task<UserDto?> GetByIdAsync(string id)
        {
            var entity = await _userRepository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.LastSeen = entity.LastSeen.ToLocalTime();
            return UserMapper.ToDto(entity);
        }

        public async Task<bool> UserExistsAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return false;

            var user = await _userRepository.GetByIdAsync(userId);
            return user != null;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            users.ForEach(u => u.LastSeen = u.LastSeen.ToLocalTime());
            return users.Select(UserMapper.ToDto).ToList();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _userRepository.DeleteAsync(id);
        }

        public async Task<bool> GiveAchievementAsync(string userId, string achievementId)
        {
            var user = await GetUserOrThrowAsync(userId);

            if (user.Achievements.Any(a => a.AchievementId == achievementId))
                return false;

            user.Achievements.Add(new UserAchievementInfo
            {
                AchievementId = achievementId,
                ReceivedAt = DateTime.UtcNow
            });

            var achievement = await _achievementRepository.GetByIdAsync(achievementId);
            if (achievement != null)
                ApplyPointsAndRecalculateLevel(user, achievement.Points);

            return await _userRepository.UpdateAsync(user);
        }

        private void ApplyPointsAndRecalculateLevel(UserEntity user, int additionalPoints)
        {
            user.Points += additionalPoints;
            user.LevelInfo = _levelService.CalculateLevelProgress(user.Points);
        }

        public async Task<UserEntity> GetUserOrThrowAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new NotFoundException($"User with ID {id} not found");

            return user;
        }
    }
}
