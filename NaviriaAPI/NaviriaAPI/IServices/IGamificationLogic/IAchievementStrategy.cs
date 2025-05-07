using NaviriaAPI.Helpers;

namespace NaviriaAPI.IServices.IGamificationLogic
{
    public interface IAchievementStrategy
    {
        AchievementTrigger Trigger { get; }
        Task<IEnumerable<string>> GetAchievementIdsAsync(string userId, object? context = null);
    }

}
