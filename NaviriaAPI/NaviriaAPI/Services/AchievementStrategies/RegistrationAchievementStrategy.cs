using NaviriaAPI.IServices;
using NaviriaAPI.IServices.IGamificationLogic;
using NaviriaAPI.Helpers;

namespace NaviriaAPI.Services.AchievementStrategies
{
    public class RegistrationAchievementStrategy : IAchievementStrategy
    {
        public AchievementTrigger Trigger => AchievementTrigger.OnRegistration;

        public Task<IEnumerable<string>> GetAchievementIdsAsync(string userId, object? context = null)
        {
            return Task.FromResult<IEnumerable<string>>([AchievementIds.Registration]);
        }
    }
}
