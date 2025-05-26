using NaviriaAPI.Helpers;
using NaviriaAPI.IServices.IGamificationLogic;
using NaviriaAPI.IRepositories;

namespace NaviriaAPI.Services.AchievementStrategies
{
    /// <summary>
    /// Strategy for the "Five Tasks in a Week" achievement.
    /// Granted if the user completes five or more tasks within the last seven days.
    /// </summary>
    public class FiveTasksInWeekAchievementStrategy : IAchievementStrategy
    {
        private readonly ITaskRepository _taskRepository;

        public FiveTasksInWeekAchievementStrategy(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public AchievementTrigger Trigger => AchievementTrigger.On5TaskInWeekTaskCompleted;

        public async Task<IEnumerable<string>> GetAchievementIdsAsync(string userId, object? context = null)
        {
            var tasks = await _taskRepository.GetCompletedTasksInLastNDaysAsync(userId, 7);

            if (tasks.Count() >= 5)
                return [AchievementIds.FiveTasksInWeekCompleted];

            return [];
        }
    }
}
