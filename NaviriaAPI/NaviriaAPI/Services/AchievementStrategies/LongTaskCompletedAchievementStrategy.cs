using NaviriaAPI.Helpers;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices.IGamificationLogic;

namespace NaviriaAPI.Services.AchievementStrategies
{
    /// <summary>
    /// Strategy for the "Long Task Completed" achievement.
    /// Granted if a task is completed at least six months after its creation date.
    /// </summary>
    public class LongTaskCompletedAchievementStrategy : IAchievementStrategy
    {
        private readonly ITaskRepository _taskRepository;

        public LongTaskCompletedAchievementStrategy(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public AchievementTrigger Trigger => AchievementTrigger.OnLongTaskCompleted;

        public async Task<IEnumerable<string>> GetAchievementIdsAsync(string userId, object? context = null)
        {
            if (context is not string taskId)
                return [];

            var task = await _taskRepository.GetByIdAsync(taskId);

            if (task == null || task.UserId != userId || !task.CompletedAt.HasValue)
                return [];

            // check for 6 month
            var months = ((task.CompletedAt.Value.Year - task.CreatedAt.Year) * 12) + (task.CompletedAt.Value.Month - task.CreatedAt.Month);
            var totalDays = (task.CompletedAt.Value - task.CreatedAt).TotalDays;
            if (months >= 6 || totalDays >= 182.5) // 182.5 ≈ 6 month
                return [AchievementIds.LongTaskCompleted];

            return [];
        }
    }
}
