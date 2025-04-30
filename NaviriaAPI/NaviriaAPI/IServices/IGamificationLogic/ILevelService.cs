using NaviriaAPI.DTOs;
using NaviriaAPI.Entities.EmbeddedEntities;

namespace NaviriaAPI.IServices.IGamificationLogic
{
    public interface ILevelService
    {
        Task<LevelProgressInfo> CalculateLevelProgressAsync(UserDto user, int additionalXp);
    }
}
