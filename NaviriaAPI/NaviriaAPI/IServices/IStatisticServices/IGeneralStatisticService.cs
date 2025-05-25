namespace NaviriaAPI.IServices.IStatisticServices
{
    /// <summary>
    /// Service interface for retrieving general application statistics.
    /// </summary>
    public interface IGeneralStatisticService
    {
        /// <summary>
        /// Gets the total number of registered users in the system.
        /// </summary>
        /// <returns>Total user count.</returns>
        Task<int> GetTotalUserCountAsync();

        /// <summary>
        /// Gets the total number of tasks created in the system.
        /// </summary>
        /// <returns>Total task count.</returns>
        Task<int> GetTotalTaskCountAsync();

        /// <summary>
        /// Gets the percentage of all tasks that have been completed.
        /// </summary>
        /// <returns>The percentage (from 0 to 100) of completed tasks.</returns>
        Task<double> GetCompletedTasksPercentageAsync();

        /// <summary>
        /// Gets the number of days since the specified user registered in the application.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <returns>Number of days since registration.</returns>
        Task<int> GetDaysSinceUserRegistrationAsync(string userId);

        /// <summary>
        /// Gets the number of days since the application's official launch ("birthday").
        /// </summary>
        /// <returns>Number of days since the app was launched.</returns>
        int GetDaysSinceAppBirthday();
    }
}
