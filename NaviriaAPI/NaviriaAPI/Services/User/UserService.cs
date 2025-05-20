using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.DTOs;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Mappings;
using Microsoft.AspNetCore.Identity;
using NaviriaAPI.Entities;
using NaviriaAPI.Services.Validation;
using NaviriaAPI.Entities.EmbeddedEntities;
using NaviriaAPI.IServices.IGamificationLogic;
using NaviriaAPI.Exceptions;
using NaviriaAPI.IServices.IJwtService;
using NaviriaAPI.Helpers;
using NaviriaAPI.IServices.IUserServices;

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
        private readonly ILogger<UserService> _logger;
        private readonly IAchievementManager _achievementManager;
        private readonly IUserCleanupService _userCleanupService;

        public UserService(
            IUserRepository userRepository,
            IPasswordHasher<UserEntity> passwordHasher,
            IConfiguration config,
            UserValidationService validation,
            IAchievementRepository achievementRepository,
            ILevelService levelService,
            IJwtService jwtService,
            ILogger<UserService> logger,
            IAchievementManager achievementManager,
            IUserCleanupService userCleanupService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _validation = validation;
            _achievementRepository = achievementRepository;
            _levelService = levelService;
            _jwtService = jwtService;
            _logger = logger;
            _achievementManager = achievementManager;
            _userCleanupService = userCleanupService;
        }

        public async Task<string> CreateAsync(UserCreateDto userDto)
        {
            await _validation.ValidateAsync(userDto);

            var entity = UserMapper.ToEntity(userDto);
            entity.Password = _passwordHasher.HashPassword(entity, userDto.Password);

            await _userRepository.CreateAsync(entity);
            await _achievementManager.EvaluateAsync(entity.Id, AchievementTrigger.OnRegistration);

            return _jwtService.GenerateUserToken(entity);
        }

        public async Task<bool> UpdateAsync(string id, UserUpdateDto userDto)
        {
            UserValidationService.ValidateAsync(userDto);

            var userDtoFromDb = await GetByIdAsync(id);
            if (userDtoFromDb == null)
                throw new NotFoundException($"User with ID {id} not found.");

            userDto.LastSeen = userDto.LastSeen.ToUniversalTime();

            if (userDtoFromDb.Points != userDto.Points)
            {
                int additionalXp = userDto.Points - userDtoFromDb.Points;
                userDto.LevelInfo = await _levelService.CalculateLevelProgressAsync(userDtoFromDb, additionalXp);
            }
            else
            {
                userDto.LevelInfo = userDtoFromDb.LevelInfo;
            }

            return await _userRepository.UpdateAsync(UserMapper.ToEntity(id, userDto));
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
            return await _userCleanupService.DeleteUserAndRelatedDataAsync(id);
        }

        public async Task<bool> GiveAchievementAsync(string userId, string achievementId)
        {
            var user = await GetUserOrThrowAsync(userId);

            if (user.Achievements.Any(a => a.AchievementId == achievementId))
            {
                _logger.LogWarning("User {UserId} already has achievement {AchievementId}.", userId, achievementId);
                throw new AlreadyExistException($"User already has achievement {achievementId}");
            }

            var achievement = await _achievementRepository.GetByIdAsync(achievementId);
            if (achievement == null)
            {
                _logger.LogWarning("Achievement with ID {AchievementId} not found.", achievementId);
                throw new NotFoundException("Achievement not found.");
            }

            user.Achievements.Add(new UserAchievementInfo
            {
                AchievementId = achievementId,
                ReceivedAt = DateTime.UtcNow,
                IsPointsReceived = false
            });

            var updated = await _userRepository.UpdateAsync(user);

            return updated;
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
