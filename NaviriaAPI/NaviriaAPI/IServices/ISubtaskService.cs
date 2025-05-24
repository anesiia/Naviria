using NaviriaAPI.DTOs.Task.Subtask.Create;
using NaviriaAPI.DTOs.Task.Subtask.Update;
using NaviriaAPI.DTOs.Task.Subtask.View;
using NaviriaAPI.Exceptions;
using System.ComponentModel.DataAnnotations;

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

        /// <summary>
        /// Marks a repeatable subtask as checked-in for the specified date.
        /// </summary>
        /// <param name="taskId">The ID of the parent task.</param>
        /// <param name="subtaskId">The ID of the repeatable subtask.</param>
        /// <param name="date">The date to mark as checked-in (will be compared by day of week).</param>
        /// <returns>
        /// The updated <see cref="SubtaskRepeatableDto"/> reflecting the current checked-in dates.
        /// </returns>
        /// <exception cref="NotFoundException">
        /// Thrown if the task or subtask is not found.
        /// </exception>
        /// <exception cref="ValidationException">
        /// Thrown if the subtask is not of repeatable type or the date is not allowed for this subtask.
        /// </exception>
        Task<SubtaskRepeatableDto> MarkRepeatableSubtaskCheckedInAsync(string taskId, string subtaskId, DateTime date);
    }
}
