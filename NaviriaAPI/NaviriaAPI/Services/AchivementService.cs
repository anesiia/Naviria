using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.IServices;
using NaviriaAPI.DTOs;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Mappings;
using NaviriaAPI.Exceptions;
using NaviriaAPI.Repositories;
using NaviriaAPI.Services.User;

namespace NaviriaAPI.Services
{
    public class AchievementService : IAchievementService
    {
        private readonly IAchievementRepository _achievementRepository;
        private readonly IUserService _userService;
        private readonly ILogger<AchievementService> _logger;
        public AchievementService(
            IAchievementRepository achievementRepository, 
            IUserService userService, 
            ILogger<AchievementService> logger)
        {
            _achievementRepository = achievementRepository;
            _userService = userService;
            _logger = logger;
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

    }
}
