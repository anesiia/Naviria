using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.IServices;
using NaviriaAPI.DTOs;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Mappings;
using NaviriaAPI.Exceptions;
using NaviriaAPI.Repositories;
using NaviriaAPI.Services.User;
using NaviriaAPI.Entities.EmbeddedEntities;
using NaviriaAPI.Entities;
using NaviriaAPI.IServices.IGamificationLogic;

namespace NaviriaAPI.Services
{
    public class AchievementService : IAchievementService
    {
        private readonly IAchievementRepository _achievementRepository;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly ILevelService _levelService;
        private readonly ILogger<AchievementService> _logger;
        public AchievementService(
            IAchievementRepository achievementRepository, 
            IUserService userService,
            IUserRepository userRepository,
            ILogger<AchievementService> logger,
            ILevelService levelService)
        {
            _achievementRepository = achievementRepository;
            _userService = userService;
            _userRepository = userRepository;
            _logger = logger;
            _levelService = levelService;
        }
        public async Task<AchievementDto> CreateAsync(AchievementCreateDto achievementDto)
        {
            var entity = AchievementMapper.ToEntity(achievementDto);
            await _achievementRepository.CreateAsync(entity);
            return AchievementMapper.ToDto(entity);
        }
        public async Task<bool> UpdateAsync(string id, AchievementUpdateDto achievementDto)
        {
            var entity = AchievementMapper.ToEntity(id, achievementDto);
            return await _achievementRepository.UpdateAsync(entity);
        }
        public async Task<AchievementDto?> GetByIdAsync(string id)
        {
            var entity = await _achievementRepository.GetByIdAsync(id);
            return entity == null ? null : AchievementMapper.ToDto(entity);
        }

        public async Task<bool> DeleteAsync(string id) =>
            await _achievementRepository.DeleteAsync(id);

        public async Task<IEnumerable<AchievementDto>> GetAllAsync()
        {
            var achievements = await _achievementRepository.GetAllAsync();
            return achievements.Select(AchievementMapper.ToDto).ToList();
        }

        public async Task<IEnumerable<AchievementDto>> GetAllUserAchievementsAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                _logger.LogWarning("GetAllUserAchievementsAsync was called with an empty or null userId.");
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));
            }

            if (!await _userService.UserExistsAsync(userId))
            {
                _logger.LogWarning("User with ID {UserId} not found.", userId);
                throw new NotFoundException($"User with ID {userId} does not exist.");
            }

            var user = await _userService.GetUserOrThrowAsync(userId);

            var achievementIds = user.Achievements
                .Select(a => a.AchievementId)
                .ToList();

            if (!achievementIds.Any())
                return Enumerable.Empty<AchievementDto>();

            var achievements = await _achievementRepository.GetManyByIdsAsync(achievementIds);

            return achievements.Select(AchievementMapper.ToDto);
        }


        public async Task<bool> AwardAchievementPointsAsync(string userId, string achievementId)
        {
            var userDto = await _userService.GetByIdAsync(userId)
                          ?? throw new NotFoundException($"User with ID {userId} not found.");

            var achievement = await _achievementRepository.GetByIdAsync(achievementId)
                              ?? throw new NotFoundException("Achievement not found");

            var userAchievement = userDto.Achievements.FirstOrDefault(a => a.AchievementId == achievementId);
            if (userAchievement == null)
                throw new InvalidOperationException("User does not have this achievement");

            if (userAchievement.IsPointsReceived)
                throw new InvalidOperationException("Points for this achievement already received");

            userDto = await ApplyPointsAndRecalculateLevelAsync(userDto, achievement.Points);
            userAchievement.IsPointsReceived = true;

            var userEntity = UserMapper.ToEntity(userId, userDto);

            var updated = await _userRepository.UpdateAsync(userEntity);

            if (!updated)
                throw new FailedToUpdateException("Failed to update user points");

            return true;
        }

        private async Task<UserDto> ApplyPointsAndRecalculateLevelAsync(UserDto user, int additionalPoints)
        {
            user.LevelInfo = await _levelService.CalculateLevelProgressAsync(user, additionalPoints);
            user.Points += additionalPoints;

            return user;
        }
    }
}
