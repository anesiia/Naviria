using NaviriaAPI.DTOs.FeaturesDTOs;

namespace NaviriaAPI.IServices.IStatisticServices
{
    /// <summary>
    /// Service interface for leaderboard statistics.
    /// </summary>
    public interface ILeaderboardService
    {
        /// <summary>
        /// Gets the top 10 users in the global leaderboard, sorted by level, points, completion rate, and achievements.
        /// </summary>
        /// <returns>List of leaderboard user DTOs.</returns>
        Task<List<LeaderboardUserDto>> GetTopLeaderboardUsersAsync();
    }
}
