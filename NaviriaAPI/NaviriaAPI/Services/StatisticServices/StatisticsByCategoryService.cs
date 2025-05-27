using NaviriaAPI.DTOs.FeaturesDTOs;
using NaviriaAPI.Entities;
using NaviriaAPI.Exceptions;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices;
using NaviriaAPI.IServices.IStatisticServices;
using NaviriaAPI.Repositories;

namespace NaviriaAPI.Services.StatisticServices
{
    public class StatisticsByCategoryService : IStatisticsByCategoryService
    {
        private readonly IStatisticRepository _statisticRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly ICategoryRepository _categoryRepository;

        public StatisticsByCategoryService(IStatisticRepository statisticRepository, IUserRepository userRepository, ITaskRepository taskRepository, ICategoryRepository categoryRepository)
        {
            _statisticRepository = statisticRepository;
            _userRepository = userRepository;
            _taskRepository = taskRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryPieChartDto>> GetUserPieChartStatsAsync(string userId)
        {
            var tasks = await _taskRepository.GetAllByUserAsync(userId);
            var categories = await _categoryRepository.GetAllAsync();
            var categoryNames = categories.ToDictionary(c => c.Id, c => c.Name);

            return CalculatePieChart(tasks, categoryNames);
        }

        public async Task<List<CategoryPieChartDto>> GetUserAndFriendsPieChartStatsAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new NotFoundException("User not found");

            var friendIds = user.Friends.Select(f => f.UserId).ToList();
            friendIds.Add(userId); // Додаємо самого юзера

            var allTasks = await _taskRepository.GetByUserIdsAsync(friendIds);
            var categories = await _categoryRepository.GetAllAsync();
            var categoryNames = categories.ToDictionary(c => c.Id, c => c.Name);

            return CalculatePieChart(allTasks, categoryNames);
        }

        public async Task<List<CategoryPieChartDto>> GetGlobalPieChartStatsAsync()
        {
            var allTasks = await _taskRepository.GetAllAsync();
            var categories = await _categoryRepository.GetAllAsync();
            var categoryNames = categories.ToDictionary(c => c.Id, c => c.Name);

            return CalculatePieChart(allTasks, categoryNames);
        }

        private List<CategoryPieChartDto> CalculatePieChart(IEnumerable<TaskEntity> tasks, Dictionary<string, string> categoryNames)
        {
            var total = tasks.Count();
            if (total == 0)
                return new List<CategoryPieChartDto>();

            var grouped = tasks
                .GroupBy(t => t.CategoryId)
                .Select(g => new CategoryPieChartDto
                {
                    CategoryId = g.Key,
                    CategoryName = categoryNames.TryGetValue(g.Key, out var name) ? name : "",
                    Value = Math.Round(((double)g.Count() / total) * 100, 0)
                })
                .OrderByDescending(x => x.Value)
                .ToList();

            return grouped;
        }


    }
}
