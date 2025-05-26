using NaviriaAPI.Helpers;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices.IGamificationLogic;

namespace NaviriaAPI.Services.AchievementStrategies
{
    /// <summary>
    /// Strategy for the "Profile Photo Uploaded" achievement.
    /// Granted when the user uploads or sets a profile photo for the first time.
    /// </summary>
    public class PhotoUploadedAchievementStrategy : IAchievementStrategy
    {
        private readonly IUserRepository _userRepository;

        public PhotoUploadedAchievementStrategy(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public AchievementTrigger Trigger => AchievementTrigger.OnPhotoUploading;

        public async Task<IEnumerable<string>> GetAchievementIdsAsync(string userId, object? context = null)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user != null && !string.IsNullOrEmpty(user.Photo))
                return [AchievementIds.PhotoUploaded];

            return [];
        }
    }
}
