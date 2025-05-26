using NaviriaAPI.Helpers;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices.IGamificationLogic;

namespace NaviriaAPI.Services.AchievementStrategies
{
    /// <summary>
    /// Strategy for the "First Task Completed" achievement.
    /// Granted when the user completes their very first task.
    /// </summary>
    public class FirstTaskCompletedAchievementStrategy : IAchievementStrategy
    {
        private readonly ITaskRepository _taskRepository;

        public FirstTaskCompletedAchievementStrategy(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public AchievementTrigger Trigger => AchievementTrigger.OnFirstTaskCompleted;

        public async Task<IEnumerable<string>> GetAchievementIdsAsync(string userId, object? context = null)
        {
            var completedCount = await _taskRepository.GetCompletedTasksCountAsync(userId);

            if (completedCount == 1)
                return [AchievementIds.FirstTaskCompleted];

            return [];
        }
    }
}
