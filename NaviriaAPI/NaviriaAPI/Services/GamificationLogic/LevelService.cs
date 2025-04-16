using NaviriaAPI.Entities.EmbeddedEntities;
using NaviriaAPI.IServices.IGamificationLogic;

namespace NaviriaAPI.Services
{
    public class LevelService : ILevelService
    {
        public LevelProgressInfo CalculateLevelProgress(int xp)
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

        private int GetXpForLevel(int level)
        {
            double rawXp = 50 * Math.Pow(level, 2.2);
            return RoundXpToNearestTen(rawXp);
        }

        private int RoundXpToNearestTen(double xp)
        {
            return (int)Math.Ceiling(xp / 10.0) * 10;
        }
    }
}
