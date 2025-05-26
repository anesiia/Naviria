using NaviriaAPI.IServices;
using NaviriaAPI.IServices.IGamificationLogic;
using NaviriaAPI.Helpers;

namespace NaviriaAPI.Services.AchievementStrategies
{
    /// <summary>
    /// Strategy for the "Registration" achievement.
    /// Granted automatically when the user completes the registration process.
    /// </summary>
    public class RegistrationAchievementStrategy : IAchievementStrategy
    {
        public AchievementTrigger Trigger => AchievementTrigger.OnRegistration;

        public Task<IEnumerable<string>> GetAchievementIdsAsync(string userId, object? context = null)
        {
            return Task.FromResult<IEnumerable<string>>([AchievementIds.Registration]);
        }
    }
}
