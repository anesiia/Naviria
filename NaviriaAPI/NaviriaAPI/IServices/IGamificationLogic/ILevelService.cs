using NaviriaAPI.Entities.EmbeddedEntities;

namespace NaviriaAPI.IServices.IGamificationLogic
{
    public interface ILevelService
    {
        LevelProgressInfo CalculateLevelProgress(int xp);
    }
}
