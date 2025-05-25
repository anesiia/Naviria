using NaviriaAPI.DTOs.FeaturesDTOs;
using NaviriaAPI.Entities;
using NaviriaAPI.Helpers;
using NaviriaAPI.IRepositories;
using System.IO.Pipes;

namespace NaviriaAPI.Services.StatisticServices
{
    public class LeaderboardService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITaskRepository _taskRepository;

        public LeaderboardService(IUserRepository userRepository, ITaskRepository taskRepository) {
            _userRepository = userRepository;
            _taskRepository = taskRepository;
        }

        public async Task<List<LeaderboardUserDto>> GetTopLeaderboardUsersAsync()
        {
            var users = (await _userRepository.GetAllAsync()).ToList();
            var tasks = (await _taskRepository.GetAllAsync()).ToList();

            var leaderList = users.Select(user => CreateLeaderboardUserDto(user, tasks)).ToList();

            return SortLeaderboard(leaderList).Take(10).ToList();
        }

        private LeaderboardUserDto CreateLeaderboardUserDto(UserEntity user, List<TaskEntity> allTasks)
        {
            var userTasks = GetUserTasks(user.Id, allTasks);
            var totalTasks = userTasks.Count;
            var completedTasks = CountCompletedTasks(userTasks);
            var completionRate = CalculateCompletionRate(totalTasks, completedTasks);

            return new LeaderboardUserDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                Nickname = user.Nickname,
                Level = user.LevelInfo.Level,
                Points = user.Points,
                CompletionRate = completionRate,
                AchievementsCount = user.Achievements?.Count ?? 0,
                Photo = user.Photo
            };
        }

        private List<TaskEntity> GetUserTasks(string userId, List<TaskEntity> allTasks)
            => allTasks.Where(t => t.UserId == userId).ToList();

        private int CountCompletedTasks(List<TaskEntity> tasks)
            => tasks.Count(t => t.Status == CurrentTaskStatus.Completed ||
                                t.Status == CurrentTaskStatus.CompletedInTime ||
                                t.Status == CurrentTaskStatus.CompletedNotInTime);

        private double CalculateCompletionRate(int totalTasks, int completedTasks)
            => totalTasks > 0 ? Math.Round((double)completedTasks / totalTasks, 2) : 0;

        private IEnumerable<LeaderboardUserDto> SortLeaderboard(IEnumerable<LeaderboardUserDto> users)
            => users.OrderByDescending(u => u.Level)
                    .ThenByDescending(u => u.Points)
                    .ThenByDescending(u => u.CompletionRate)
                    .ThenByDescending(u => u.AchievementsCount);

    }
}
