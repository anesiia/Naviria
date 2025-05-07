using NaviriaAPI.Entities.EmbeddedEntities;
using NaviriaAPI.Exceptions;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices.IGamificationLogic;

namespace NaviriaAPI.Services.GamificationLogic
{
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

        public async Task GiveAsync(string userId, string achievementId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new NotFoundException("User not found");

            if (user.Achievements.Any(a => a.AchievementId == achievementId))
                return;

            var achievement = await _achievementRepository.GetByIdAsync(achievementId);
            if (achievement == null) throw new NotFoundException("Achievement not found");

            _logger.LogInformation("Грант досягнення {AchievementId} для користувача {UserId}", achievementId, userId);

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
