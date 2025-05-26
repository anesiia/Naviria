using NaviriaAPI.Helpers;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices.IGamificationLogic;

namespace NaviriaAPI.Services.AchievementStrategies
{
    /// <summary>
    /// Strategy for the "Five Friends Added" achievement.
    /// Granted when the user has five or more friends in their profile.
    /// </summary>
    public class FiveFriendsAddedAchievementStrategy : IAchievementStrategy
    {
        private readonly IUserRepository _userRepository;

        public FiveFriendsAddedAchievementStrategy(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public AchievementTrigger Trigger => AchievementTrigger.OnAdding5Friends;

        public async Task<IEnumerable<string>> GetAchievementIdsAsync(string userId, object? context = null)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user != null && user.Friends.Count >= 5)
                return [AchievementIds.FiveFriendAdded];

            return [];
        }
    }
}
