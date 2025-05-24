using NaviriaAPI.DTOs.Task.Update;
using NaviriaAPI.DTOs.FeaturesDTOs;
using NaviriaAPI.Exceptions;
using NaviriaAPI.DTOs.Task.Create;
using NaviriaAPI.DTOs.Task.View;

namespace NaviriaAPI.IServices
{
    /// <summary>
    /// Service interface for managing user tasks and task-related operations.
    /// </summary>
    public interface ITaskService
    {
        /// <summary>
        /// Retrieves all tasks for the specified user.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <returns>A list of TaskDto objects.</returns>
        /// <exception cref="NotFoundException">Thrown if the user does not exist or has no tasks.</exception>
        Task<IEnumerable<TaskDto>> GetAllByUserAsync(string userId);

        /// <summary>
        /// Retrieves a task by its ID.
        /// </summary>
        /// <param name="id">Task ID.</param>
        /// <returns>The TaskDto object, or null if not found.</returns>
        /// <exception cref="NotFoundException">Thrown if the task does not exist.</exception>
        Task<TaskDto?> GetByIdAsync(string id);

        /// <summary>
        /// Creates a new task.
        /// </summary>
        /// <param name="dto">Task creation data transfer object.</param>
        /// <returns>The created TaskDto.</returns>
        /// <exception cref="NotFoundException">Thrown if the user does not exist.</exception>
        /// <exception cref="SuspiciousMessageException">Thrown if the title or description contains forbidden content.</exception>
        Task<TaskDto> CreateAsync(TaskCreateDto dto);

        /// <summary>
        /// Updates an existing task.
        /// </summary>
        /// <param name="id">Task ID.</param>
        /// <param name="dto">Task update data transfer object.</param>
        /// <returns>True if updated successfully, otherwise false.</returns>
        /// <exception cref="NotFoundException">Thrown if the task does not exist.</exception>
        /// <exception cref="SuspiciousMessageException">Thrown if the title or description contains forbidden content.</exception>
        Task<bool> UpdateAsync(string id, TaskUpdateDto dto);

        /// <summary>
        /// Deletes a task by its ID.
        /// </summary>
        /// <param name="id">Task ID.</param>
        /// <returns>True if deleted successfully, otherwise false.</returns>
        /// <exception cref="NotFoundException">Thrown if the task does not exist.</exception>
        Task<bool> DeleteAsync(string id);

        /// <summary>
        /// Retrieves all tasks for a user, grouped by folders.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <returns>A collection of FolderWithTasksDto with grouped tasks.</returns>
        /// <exception cref="NotFoundException">Thrown if the user does not exist or has no tasks.</exception>
        Task<IEnumerable<FolderWithTasksDto>> GetGroupedTasksByFoldersAsync(string userId);

        /// <summary>
        /// Retrieves all tasks with a deadline on the specified date.
        /// </summary>
        /// <param name="deadlineDate">Deadline date.</param>
        /// <returns>A list of TaskDto with the specified deadline.</returns>
        /// <exception cref="NotFoundException">Thrown if no tasks are found with the specified deadline.</exception>
        Task<IEnumerable<TaskDto>> GetTasksWithDeadlineAsync(DateTime deadlineDate);
    }
}
