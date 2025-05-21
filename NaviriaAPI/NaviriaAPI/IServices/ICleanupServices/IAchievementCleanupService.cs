namespace NaviriaAPI.IServices.ICleanupServices
{
    /// <summary>
    /// Service for cascading deletion of achievements.
    /// Deletes the achievement and removes it from all users who had this achievement.
    /// </summary>
    public interface IAchievementCleanupService
    {
        /// <summary>
        /// Deletes the achievement and removes it from all users' achievement lists.
        /// </summary>
        /// <param name="achievementId">The identifier of the achievement to delete.</param>
        /// <returns>True if deletion was successful; otherwise, false.</returns>
        Task<bool> DeleteAchievementAndRemoveFromUsersAsync(string achievementId);
    }

}
