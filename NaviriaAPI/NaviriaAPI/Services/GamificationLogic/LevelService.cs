using NaviriaAPI.Entities.EmbeddedEntities;
using NaviriaAPI.IServices.IGamificationLogic;
using NaviriaAPI.IServices;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.Constants;
using NaviriaAPI.DTOs;

namespace NaviriaAPI.Services
{
    public class LevelService : ILevelService
    {
        private readonly INotificationService _notificationService;

        public LevelService(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<LevelProgressInfo> CalculateLevelProgressAsync(UserDto user, int additionalXp)
        {
            if (user == null)
                throw new ArgumentNullException("User is not found");

            int oldXp = user.Points;
            int newXp = oldXp + additionalXp;

            int oldLevel = GetLevelByXp(oldXp);
            int newLevel = GetLevelByXp(newXp);

            var levelProgress = BuildLevelProgress(newXp);

            if (newLevel > oldLevel)
            {
                var message = GetLevelUpMessage(newLevel);

                await _notificationService.CreateAsync(new NotificationCreateDto
                {
                    UserId = user.Id,
                    Text = message,
                    RecievedAt = DateTime.UtcNow
                });
            }

            return levelProgress;
        }


        private LevelProgressInfo BuildLevelProgress(int xp)
        {
            int level = 0;
            int currentXp = 0;
            int nextXp = 0;

            while (true)
            {
                nextXp = GetXpForLevel(level + 1);
                if (xp < nextXp) break;
                currentXp = nextXp;
                level++;
            }

            double progress = (double)(xp - currentXp) / (nextXp - currentXp);

            return new LevelProgressInfo
            {
                Level = level,
                TotalXp = xp,
                XpForNextLevel = nextXp,
                Progress = Math.Round(progress, 2)
            };
        }

        private int GetLevelByXp(int xp)
        {
            int level = 0;
            int nextXp = 0;

            while (true)
            {
                nextXp = GetXpForLevel(level + 1);
                if (xp < nextXp) break;
                level++;
            }

            return level;
        }

        private int GetXpForLevel(int level)
        {
            double rawXp = 50 * Math.Pow(level, 2.2);
            return (int)Math.Ceiling(rawXp / 10.0) * 10;
        }

        

        private string GetLevelUpMessage(int level)
        {
            return LevelCongratMessages.Messages.TryGetValue(level, out var specialMessage)
                ? specialMessage
                : $"🎉 Вітаємо! Ви досягли {level} рівня!";
        }

}
}
