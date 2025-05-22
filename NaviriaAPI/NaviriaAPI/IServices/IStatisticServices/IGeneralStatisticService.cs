namespace NaviriaAPI.IServices.IStatisticServices
{
    public interface IGeneralStatisticService
    {
        Task<int> GetTotalUserCountAsync();
        Task<int> GetTotalTaskCountAsync();
        Task<double> GetCompletedTasksPercentageAsync();
        Task<int> GetDaysSinceUserRegistrationAsync(string userId);
        int GetDaysSinceAppBirthday();
    }
}
