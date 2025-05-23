using NaviriaAPI.Exceptions;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices.IStatisticServices;
using NaviriaAPI.Repositories;
using NaviriaAPI.Constants;

namespace NaviriaAPI.Services.StatisticServices
{
    public class GeneralStatisticsService : IGeneralStatisticService
    {
        private readonly IStatisticRepository _statisticRepository;
        private readonly IUserRepository _userRepository;

        public GeneralStatisticsService(IStatisticRepository statisticRepository, IUserRepository userRepository)
        {
            _statisticRepository = statisticRepository;
            _userRepository = userRepository;
        }

        public async Task<int> GetTotalUserCountAsync()
            => await _statisticRepository.GetTotalUserCountAsync();

        public async Task<int> GetTotalTaskCountAsync()
            => await _statisticRepository.GetTotalTaskCountAsync();

        public async Task<double> GetCompletedTasksPercentageAsync()
        {
            var total = await _statisticRepository.GetTotalTaskCountAsync();
            if (total == 0) return 0;

            var completed = await _statisticRepository.GetCompletedTaskCountAsync();
            return (int)Math.Round(100.0 * completed / total);
        }


        public async Task<int> GetDaysSinceUserRegistrationAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new NotFoundException($"User with ID {userId} not found.");

            var days = (DateTime.UtcNow.Date - user.RegitseredAt.Date).Days;
            return Math.Max(days, 0);
        }

        public int GetDaysSinceAppBirthday()
        {
            return (DateTime.UtcNow.Date - ProjectConstants.AppBirthday).Days;
        }
    }

}
