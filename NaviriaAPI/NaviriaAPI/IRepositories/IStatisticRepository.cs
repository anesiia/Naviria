using System;
using System.Threading.Tasks;

namespace NaviriaAPI.IRepositories
{
    public interface IStatisticRepository
    {
        Task<int> GetTotalUserCountAsync();
        Task<int> GetTotalTaskCountAsync();
        Task<int> GetCompletedTaskCountAsync();
        Task<DateTime?> GetUserRegistrationDateAsync(string userId);
    }
}
