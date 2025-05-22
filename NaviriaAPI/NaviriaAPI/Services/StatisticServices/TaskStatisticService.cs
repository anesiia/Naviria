using NaviriaAPI.IRepositories;
using NaviriaAPI.Entities.EmbeddedEntities.Subtasks;
using NaviriaAPI.IServices.IStatisticServices;

namespace NaviriaAPI.Services.StatisticServices
{
    public class TaskStatisticService : ITaskStatisticService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskStatisticService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<int> GetTotalCheckedInDaysCountForUserAsync(string userId)
        {
            var allTasks = await _taskRepository.GetAllByUserAsync(userId);

            return allTasks
                .SelectMany(t => t.Subtasks)
                .OfType<SubtaskRepeatable>()
                .Sum(r => r.CheckedInDays?.Count ?? 0);
        }

        public async Task<int> GetCheckedInDaysCountForSubtaskAsync(string userId, string subtaskId)
        {
            var allTasks = await _taskRepository.GetAllByUserAsync(userId);
            var subtask = allTasks
                .SelectMany(t => t.Subtasks)
                .OfType<SubtaskRepeatable>()
                .FirstOrDefault(r => r.Id == subtaskId);

            return subtask?.CheckedInDays?.Count ?? 0;
        }
    }
}
