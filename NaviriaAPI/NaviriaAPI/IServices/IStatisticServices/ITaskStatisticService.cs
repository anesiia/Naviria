namespace NaviriaAPI.IServices.IStatisticServices
{
    /// <summary>
    /// Provides methods for calculating statistics related to tasks and subtasks for users.
    /// </summary>
    public interface ITaskStatisticService
    {
        /// <summary>
        /// Gets the total number of checked-in days for all repeatable subtasks
        /// across all tasks belonging to the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>
        /// The total count of checked-in days for all repeatable subtasks.
        /// Returns 0 if the user has no repeatable subtasks or tasks.
        /// </returns>
        Task<int> GetTotalCheckedInDaysCountForUserAsync(string userId);

        /// <summary>
        /// Gets the number of checked-in days for a specific repeatable subtask
        /// belonging to the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="subtaskId">The unique identifier of the subtask.</param>
        /// <returns>
        /// The count of checked-in days for the specified repeatable subtask.
        /// Returns 0 if the subtask is not found or is not of repeatable type.
        /// </returns>
        Task<int> GetCheckedInDaysCountForSubtaskAsync(string userId, string subtaskId);
    }
}

