using NaviriaAPI.IRepositories;
using Microsoft.Extensions.Logging;
using NaviriaAPI.IServices.ICleanupServices;
using NaviriaAPI.Exceptions;

namespace NaviriaAPI.Services.CleanupServices
{
    public class AchievementCleanupService : IAchievementCleanupService
    {
        private readonly IAchievementRepository _achievementRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AchievementCleanupService> _logger;

        public AchievementCleanupService(
            IAchievementRepository achievementRepository,
            IUserRepository userRepository,
            ILogger<AchievementCleanupService> logger)
        {
            _achievementRepository = achievementRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteAchievementAndRemoveFromUsersAsync(string achievementId)
        {
            var achievement = await _achievementRepository.GetByIdAsync(achievementId);
            if (achievement == null)
                throw new NotFoundException($"Achievement with ID {achievementId} not found.");;

            // Remove achievement from all users
            await _userRepository.RemoveAchievementFromAllUsersAsync(achievementId);

            // Delete achievement from the achievements collection
            var deleted = await _achievementRepository.DeleteAsync(achievementId);

            return deleted;
        }
    }

}
