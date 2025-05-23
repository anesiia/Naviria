using NaviriaAPI.Entities;
using NaviriaAPI.Entities.EmbeddedEntities.Subtasks;

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

        Task<List<TaskEntity>> GetOverdueTasksAsync(DateTime now);

        Task<List<SubtaskRepeatable>> GetAllRepeatableSubtasksByUserAsync(string userId);


    }
}
