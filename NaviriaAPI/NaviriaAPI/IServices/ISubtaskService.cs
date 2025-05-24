using NaviriaAPI.DTOs.Task.Subtask.Create;
using NaviriaAPI.DTOs.Task.Subtask.Update;

namespace NaviriaAPI.IServices
{
    /// <summary>
    /// Provides methods for managing subtasks (add, update, remove) within a specific task.
    /// </summary>
    public interface ISubtaskService
    {
        /// <summary>
        /// Adds a new subtask to the specified task.
        /// </summary>
        /// <param name="taskId">The unique identifier of the task to which the subtask will be added.</param>
        /// <param name="subtaskDto">The DTO containing the data for the new subtask.</param>
        /// <returns>
        /// A boolean indicating whether the operation was successful.
        /// Returns <c>true</c> if the subtask was added; <c>false</c> if the task was not found.
        /// </returns>
        Task<bool> AddSubtaskAsync(string taskId, SubtaskCreateDtoBase subtaskDto);

        /// <summary>
        /// Updates an existing subtask within the specified task.
        /// </summary>
        /// <param name="taskId">The unique identifier of the task containing the subtask.</param>
        /// <param name="subtaskId">The unique identifier of the subtask to update.</param>
        /// <param name="subtaskDto">The DTO containing the updated data for the subtask.</param>
        /// <returns>
        /// A boolean indicating whether the operation was successful.
        /// Returns <c>true</c> if the subtask was updated;
        /// <c>false</c> if the task or subtask was not found.
        /// </returns>
        Task<bool> UpdateSubtaskAsync(string taskId, string subtaskId, SubtaskUpdateDtoBase subtaskDto);

        /// <summary>
        /// Removes a subtask from the specified task.
        /// </summary>
        /// <param name="taskId">The unique identifier of the task from which the subtask will be removed.</param>
        /// <param name="subtaskId">The unique identifier of the subtask to remove.</param>
        /// <returns>
        /// A boolean indicating whether the operation was successful.
        /// Returns <c>true</c> if the subtask was removed;
        /// <c>false</c> if the task or subtask was not found.
        /// </returns>
        Task<bool> RemoveSubtaskAsync(string taskId, string subtaskId);
    }
}
