using NaviriaAPI.DTOs.FeaturesDTOs;

namespace NaviriaAPI.IServices.IStatisticServices
{
    /// <summary>
    /// Service interface for generating task completion statistics by date (for line chart visualization).
    /// </summary>
    public interface ITaskStatisticByDateService
    {
        /// <summary>
        /// Retrieves statistics of completed tasks per month for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user for whom statistics are requested.</param>
        /// <returns>A list of objects containing the month and the count of completed tasks for that month.</returns>
        Task<List<TasksCompletedPerMonthDto>> GetUserTasksCompletedPerMonthAsync(string userId);

        /// <summary>
        /// Retrieves statistics of completed tasks per month for a user and all their friends.
        /// </summary>
        /// <param name="userId">The ID of the user whose statistics, together with their friends, are requested.</param>
        /// <returns>A list of objects containing the month and the count of completed tasks for that month (user + friends).</returns>
        Task<List<TasksCompletedPerMonthDto>> GetUserAndFriendsTasksCompletedPerMonthAsync(string userId);

        /// <summary>
        /// Retrieves global statistics of completed tasks per month for all users in the system.
        /// </summary>
        /// <returns>A list of objects containing the month and the count of completed tasks for that month (global statistics).</returns>
        Task<List<TasksCompletedPerMonthDto>> GetGlobalTasksCompletedPerMonthAsync();
    }
}
