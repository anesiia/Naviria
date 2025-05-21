using NaviriaAPI.Entities;

namespace NaviriaAPI.IRepositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskEntity>> GetAllByUserAsync(string userId);
        Task<TaskEntity?> GetByIdAsync(string id);
        Task CreateAsync(TaskEntity entity);
        Task<bool> UpdateAsync(TaskEntity entity);
        Task<bool> DeleteAsync(string id);
        Task DeleteManyByUserIdAsync(string userId);
        Task DeleteManyByCategoryIdAsync(string categoryId);
        Task<IEnumerable<TaskEntity>> GetTasksWithDeadlineOnDateAsync(DateTime deadlineDate);
        Task<List<string>> GetUserIdsByCategoryAsync(string categoryId); 

    }
}
