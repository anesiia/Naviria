namespace NaviriaAPI.IServices.IGamificationLogic
{
    public interface IAchievementGranter
    {
        Task GiveAsync(string userId, string achievementId);
    }
}
