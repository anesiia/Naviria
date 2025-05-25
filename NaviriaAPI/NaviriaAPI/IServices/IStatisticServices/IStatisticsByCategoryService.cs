using NaviriaAPI.DTOs.FeaturesDTOs;
using NaviriaAPI.Exceptions;

namespace NaviriaAPI.IServices.IStatisticServices
{
    /// <summary>
    /// Service interface for retrieving task category statistics (for pie chart visualization).
    /// </summary>
    public interface IStatisticsByCategoryService
    {
        /// <summary>
        /// Gets pie chart statistics for the user's tasks grouped by category.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <returns>A list of <see cref="CategoryPieChartDto"/> representing category proportions for the user.</returns>
        /// <exception cref="NotFoundException">Thrown if the user has no tasks or does not exist.</exception>
        Task<List<CategoryPieChartDto>> GetUserPieChartStatsAsync(string userId);

        /// <summary>
        /// Gets pie chart statistics for the user's and their friends' tasks grouped by category.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <returns>A list of <see cref="CategoryPieChartDto"/> representing combined category proportions for the user and friends.</returns>
        /// <exception cref="NotFoundException">Thrown if the user and friends have no tasks or do not exist.</exception>
        Task<List<CategoryPieChartDto>> GetUserAndFriendsPieChartStatsAsync(string userId);

        /// <summary>
        /// Gets global pie chart statistics for all users' tasks grouped by category.
        /// </summary>
        /// <returns>A list of <see cref="CategoryPieChartDto"/> representing category proportions for all users.</returns>
        Task<List<CategoryPieChartDto>> GetGlobalPieChartStatsAsync();
    }
}
