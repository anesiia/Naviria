using NaviriaAPI.Entities;
using NaviriaAPI.Entities.EmbeddedEntities.Subtasks;
using System.Threading.Tasks;

namespace NaviriaAPI.IRepositories
{
    /// <summary>
    /// Repository interface for managing tasks in the database.
    /// Provides CRUD operations and various search and bulk delete methods for tasks.
    /// </summary>
    public interface ITaskRepository
    {
        /// <summary>
        /// Retrieves all tasks belonging to a specific user.
        /// </summary>
        /// <param name="userId">The identifier of the user whose tasks are to be retrieved.</param>
        /// <returns>An enumerable collection of task entities.</returns>
        Task<IEnumerable<TaskEntity>> GetAllByUserAsync(string userId);

        /// <summary>
        /// Retrieves a task by its unique identifier.
        /// </summary>
        /// <param name="id">The identifier of the task.</param>
        /// <returns>The matching task entity, or null if not found.</returns>
        Task<TaskEntity?> GetByIdAsync(string id);

        /// <summary>
        /// Retrieves all tasks that belong to a set of user IDs.
        /// </summary>
        /// <param name="userIds">A collection of user IDs whose tasks are to be retrieved.</param>
        /// <returns>An enumerable collection of <see cref="TaskEntity"/> belonging to the specified users.</returns>
        Task<IEnumerable<TaskEntity>> GetByUserIdsAsync(IEnumerable<string> userIds);

        /// <summary>
        /// Retrieves all tasks in the database.
        /// </summary>
        /// <returns>An enumerable collection of all <see cref="TaskEntity"/> in the database.</returns>
        Task<IEnumerable<TaskEntity>> GetAllAsync();

        /// <summary>
        /// Creates a new task in the database.
        /// </summary>
        /// <param name="entity">The task entity to create.</param>
        Task CreateAsync(TaskEntity entity);

        /// <summary>
        /// Updates an existing task in the database.
        /// </summary>
        /// <param name="entity">The updated task entity.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        Task<bool> UpdateAsync(TaskEntity entity);

        /// <summary>
        /// Deletes a task by its unique identifier.
        /// </summary>
        /// <param name="id">The identifier of the task to delete.</param>
        /// <returns>True if the task was deleted successfully; otherwise, false.</returns>
        Task<bool> DeleteAsync(string id);

        /// <summary>
        /// Deletes all tasks that belong to a specific user.
        /// </summary>
        /// <param name="userId">The identifier of the user whose tasks will be deleted.</param>
        Task DeleteManyByUserIdAsync(string userId);

        /// <summary>
        /// Deletes all tasks that have the specified category.
        /// </summary>
        /// <param name="categoryId">The identifier of the category.</param>
        Task DeleteManyByCategoryIdAsync(string categoryId);

        /// <summary>
        /// Retrieves all tasks with a deadline set for a specific date and with notifications enabled.
        /// </summary>
        /// <param name="deadlineDate">The date to check for deadlines (date part only).</param>
        /// <returns>An enumerable collection of task entities.</returns>
        Task<IEnumerable<TaskEntity>> GetTasksWithDeadlineOnDateAsync(DateTime deadlineDate);

        /// <summary>
        /// Retrieves a list of user IDs who have tasks with the specified category.
        /// </summary>
        /// <param name="categoryId">The identifier of the category.</param>
        /// <returns>A list of user IDs.</returns>
        Task<List<string>> GetUserIdsByCategoryAsync(string categoryId);

        /// <summary>
        /// Deletes all tasks belonging to a specific folder.
        /// </summary>
        /// <param name="folderId">The ID of the folder.</param>
        Task DeleteManyByFolderIdAsync(string folderId);

        /// <summary>
        /// Retrieves all tasks that are overdue (their deadline is before the specified datetime and they are still in progress).
        /// </summary>
        /// <param name="now">The current date and time used to determine overdue tasks.</param>
        /// <returns>A list of <see cref="TaskEntity"/> that are overdue.</returns>
        Task<List<TaskEntity>> GetOverdueTasksAsync(DateTime now);

        /// <summary>
        /// Retrieves all repeatable subtasks for a specific user by aggregating repeatable subtasks from all the user's tasks.
        /// </summary>
        /// <param name="userId">The identifier of the user.</param>
        /// <returns>A list of <see cref="SubtaskRepeatable"/> belonging to the user's tasks.</returns>
        Task<List<SubtaskRepeatable>> GetAllRepeatableSubtasksByUserAsync(string userId);

        /// <summary>
        /// Retrieves all tasks completed by a specific user within the last N days.
        /// </summary>
        /// <param name="userId">The identifier of the user.</param>
        /// <param name="days">The number of days to look back from the current date.</param>
        /// <returns>An enumerable collection of <see cref="TaskEntity"/> completed within the specified time frame.</returns>
        Task<IEnumerable<TaskEntity>> GetCompletedTasksInLastNDaysAsync(string userId, int days);

        /// <summary>
        /// Retrieves the total number of tasks completed by a specific user.
        /// </summary>
        /// <param name="userId">The identifier of the user.</param>
        /// <returns>The count of completed tasks.</returns>
        Task<int> GetCompletedTasksCountAsync(string userId);

    }
}
