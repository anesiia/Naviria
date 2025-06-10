using NaviriaAPI.Entities.EmbeddedEntities;
using NaviriaAPI.Exceptions;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices.IGamificationLogic;

namespace NaviriaAPI.Services.GamificationLogic
{
    /// <summary>
    /// Service for granting achievements to users.
    /// Adds achievement information to the user's profile.
    /// </summary>
    public class AchievementGranter : IAchievementGranter
    {
        private readonly IUserRepository _userRepository;
        private readonly IAchievementRepository _achievementRepository;
        private readonly ILogger<AchievementGranter> _logger;

        public AchievementGranter(
            IUserRepository userRepository,
            IAchievementRepository achievementRepository, 
            ILogger<AchievementGranter> logger)
        {
            _userRepository = userRepository;
            _achievementRepository = achievementRepository;
            _logger = logger;
        }

        /// <summary>
        /// Grants the specified achievement to the user if it has not already been received.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="achievementId">The achievement ID.</param>
        public async Task GiveAsync(string userId, string achievementId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new NotFoundException("User not found");

            if (user.Achievements.Any(a => a.AchievementId == achievementId))
                return;

            var achievement = await _achievementRepository.GetByIdAsync(achievementId);
            if (achievement == null) throw new NotFoundException("Achievement not found");

            _logger.LogInformation("Achievement {AchievementId} for user {UserId}", achievementId, userId);

            user.Achievements.Add(new UserAchievementInfo
            {
                AchievementId = achievementId,
                ReceivedAt = DateTime.UtcNow,
                IsPointsReceived = false
            });

            await _userRepository.UpdateAsync(user);
        }
    }

}
