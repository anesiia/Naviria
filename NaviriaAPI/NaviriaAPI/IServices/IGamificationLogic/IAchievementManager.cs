using NaviriaAPI.Helpers;

namespace NaviriaAPI.IServices.IGamificationLogic
{
    public interface IAchievementManager
    {
        Task EvaluateAsync(string userId, AchievementTrigger trigger, object? context = null);
    }

}
