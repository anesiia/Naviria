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
using NaviriaAPI.IServices.ICleanupServices;

namespace NaviriaAPI.Services.User
{
    /// <summary>
    /// Provides user management operations: CRUD, authentication, achievements, and level handling.
    /// </summary>
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

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public UserService(
            IUserRepository userRepository,
            IPasswordHasher<UserEntity> passwordHasher,
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

        /// <summary>
        /// Registers a new user, hashes the password, and returns a JWT.
        /// </summary>
        public async Task<string> CreateAsync(UserCreateDto userDto)
        {
            await _validation.ValidateAsync(userDto);

            var entity = UserMapper.ToEntity(userDto);
            entity.Password = _passwordHasher.HashPassword(entity, userDto.Password);

            await _userRepository.CreateAsync(entity);
            await _achievementManager.EvaluateAsync(entity.Id, AchievementTrigger.OnRegistration);

            return _jwtService.GenerateUserToken(entity);
        }

        /// <summary>
        /// Updates a user and recalculates XP/level if needed.
        /// </summary>
        public async Task<bool> UpdateAsync(string id, UserUpdateDto userDto)
        {
            UserValidationService.ValidateAsync(userDto);

            var userDtoFromDb = await GetByIdAsync(id);
            if (userDtoFromDb == null)
                throw new NotFoundException($"User with ID {id} not found.");

            userDto.LastSeen = userDto.LastSeen.ToUniversalTime();

            // Recalculate level info if points changed
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

        /// <summary>
        /// Gets user DTO by user ID.
        /// </summary>
        public async Task<UserDto?> GetByIdAsync(string id)
        {
            var entity = await _userRepository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.LastSeen = entity.LastSeen.ToLocalTime();
            return UserMapper.ToDto(entity);
        }

        /// <summary>
        /// Checks if a user exists by their ID.
        /// </summary>
        public async Task<bool> UserExistsAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return false;

            var user = await _userRepository.GetByIdAsync(userId);
            return user != null;
        }

        /// <summary>
        /// Gets all users in the system as DTOs.
        /// </summary>
        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            users.ForEach(u => u.LastSeen = u.LastSeen.ToLocalTime());
            return users.Select(UserMapper.ToDto).ToList();
        }

        /// <summary>
        /// Deletes user and all related data.
        /// </summary>
        public async Task<bool> DeleteAsync(string id)
        {
            return await _userCleanupService.DeleteUserAndRelatedDataAsync(id);
        }

        /// <summary>
        /// Gives an achievement to a user if not already present.
        /// </summary>
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

            return await _userRepository.UpdateAsync(user);
        }

        /// <summary>
        /// Gets user entity by ID or throws NotFoundException.
        /// </summary>
        public async Task<UserEntity> GetUserOrThrowAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new NotFoundException($"User with ID {id} not found");

            return user;
        }
    }
}
