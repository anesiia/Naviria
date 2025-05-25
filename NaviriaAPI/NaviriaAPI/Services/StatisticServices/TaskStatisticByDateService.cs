using NaviriaAPI.DTOs.FeaturesDTOs;
using NaviriaAPI.Entities;
using NaviriaAPI.Exceptions;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices.IStatisticServices;

namespace NaviriaAPI.Services.StatisticServices
{
    public class TaskStatisticByDateService : ITaskStatisticByDateService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITaskRepository _taskRepository;

        public TaskStatisticByDateService(IUserRepository userRepository, ITaskRepository taskRepository)
        {
            _userRepository = userRepository;
            _taskRepository = taskRepository;
        }

        /// <inheritdoc />
        public async Task<List<TasksCompletedPerMonthDto>> GetUserTasksCompletedPerMonthAsync(string userId)
        {
            var tasks = await _taskRepository.GetAllByUserAsync(userId);
            return AggregateCompletedTasksByMonth(tasks);
        }

        /// <inheritdoc />
        public async Task<List<TasksCompletedPerMonthDto>> GetUserAndFriendsTasksCompletedPerMonthAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new NotFoundException("User not found");

            var friendIds = user.Friends.Select(f => f.UserId).ToList();
            friendIds.Add(userId);

            var allTasks = await _taskRepository.GetByUserIdsAsync(friendIds);
            return AggregateCompletedTasksByMonth(allTasks);
        }

        /// <inheritdoc />
        public async Task<List<TasksCompletedPerMonthDto>> GetGlobalTasksCompletedPerMonthAsync()
        {
            var allTasks = await _taskRepository.GetAllAsync();
            return AggregateCompletedTasksByMonth(allTasks);
        }

        private List<TasksCompletedPerMonthDto> AggregateCompletedTasksByMonth(IEnumerable<TaskEntity> tasks)
        {
            return tasks
                .Where(t => t.CompletedAt.HasValue)
                .GroupBy(t => new { t.CompletedAt.Value.Year, t.CompletedAt.Value.Month })

                .Select(g => new TasksCompletedPerMonthDto
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    CompletedCount = g.Count()
                })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .ToList();
        }
    }

}
