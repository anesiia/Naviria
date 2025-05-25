using NaviriaAPI.DTOs.User;
using NaviriaAPI.Entities.EmbeddedEntities;

namespace NaviriaAPI.IServices.IGamificationLogic
{
    /// <summary>
    /// Service for calculating user level progress and XP requirements.
    /// </summary>
    public interface ILevelService
    {
        /// <summary>
        /// Calculates the initial level progress for a new user based on starting experience points.
        /// </summary>
        /// <param name="totalXp">The total experience points to assign to the new user (e.g., starting XP).</param>
        /// <returns>
        /// A <see cref="LevelProgressInfo"/> object representing the calculated level and progress towards the next level.
        /// </returns>
        LevelProgressInfo CalculateFirstLevelProgress(int totalXp);

        /// <summary>
        /// Calculates the user's level progress after gaining additional experience points.
        /// May also trigger notifications if the user's level has increased.
        /// </summary>
        /// <param name="user">The user data transfer object (<see cref="UserDto"/>) containing current XP and user information.</param>
        /// <param name="additionalXp">The number of experience points to add to the user's current total.</param>
        /// <returns>
        /// A <see cref="LevelProgressInfo"/> object representing the updated level and progress.
        /// </returns>
        Task<LevelProgressInfo> CalculateLevelProgressAsync(UserDto user, int additionalXp);
    }
}
