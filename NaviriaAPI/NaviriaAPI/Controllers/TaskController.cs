﻿using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.DTOs.Task.Update;
using NaviriaAPI.IServices;
using Microsoft.AspNetCore.Authorization;
using NaviriaAPI.DTOs.Task.Create;
using NaviriaAPI.DTOs.TaskDtos;
using NaviriaAPI.Exceptions;
using NaviriaAPI.DTOs.FeaturesDTOs;

namespace NaviriaAPI.Controllers
{
    /// <summary>
    /// Controller for managing user tasks.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TaskController> _logger;

        public TaskController(ITaskService taskService, ILogger<TaskController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all tasks for the specified user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>A list of the user's tasks.</returns>
        /// <response code="200">Returns a list of tasks.</response>
        /// <response code="500">If an error occurs.</response>
        [Authorize]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllByUser(string userId)
        {
            try
            {
                var result = await _taskService.GetAllByUserAsync(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get tasks for user with ID {UserId}", userId);
                return StatusCode(500, $"Failed to get tasks for user with ID {userId}");
            }
        }

        /// <summary>
        /// Gets a task by its ID.
        /// </summary>
        /// <param name="id">The task ID.</param>
        /// <returns>The task with the specified ID.</returns>
        /// <response code="200">Returns the task.</response>
        /// <response code="404">If the task is not found.</response>
        /// <response code="500">If an error occurs.</response>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Task ID is required.");

            try
            {
                var result = await _taskService.GetByIdAsync(id);
                return result is null ? NotFound() : Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get task with ID {TaskId}", id);
                return StatusCode(500, $"Failed to get task with ID {id}");
            }
        }

        /// <summary>
        /// Creates a new task.
        /// </summary>
        /// <param name="dto">The task creation data transfer object.</param>
        /// <returns>The created task.</returns>
        /// <response code="201">Returns the created task.</response>
        /// <response code="400">If the model state is invalid.</response>
        /// <response code="500">If an error occurs.</response>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaskCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _taskService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create task");
                return StatusCode(500, "Failed to create task");
            }
        }

        /// <summary>
        /// Updates an existing task.
        /// </summary>
        /// <param name="id">The task ID.</param>
        /// <param name="dto">The task update data transfer object.</param>
        /// <returns>No content if the update is successful.</returns>
        /// <response code="204">If the update was successful.</response>
        /// <response code="400">If the model state is invalid or ID is missing.</response>
        /// <response code="404">If the task is not found.</response>
        /// <response code="500">If an error occurs.</response>
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] TaskUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Task ID is required.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _taskService.UpdateAsync(id, dto);
                return updated ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update task with ID {TaskId}", id);
                return StatusCode(500, $"Failed to update task with ID {id}");
            }
        }

        /// <summary>
        /// Deletes a task by its ID.
        /// </summary>
        /// <param name="id">The task ID.</param>
        /// <returns>No content if deleted.</returns>
        /// <response code="204">If the task was deleted.</response>
        /// <response code="400">If the ID is missing.</response>
        /// <response code="404">If the task is not found.</response>
        /// <response code="500">If an error occurs.</response>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Task ID is required.");

            try
            {
                var deleted = await _taskService.DeleteAsync(id);
                return deleted ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete task with ID {TaskId}", id);
                return StatusCode(500, $"Failed to delete task with ID {id}");
            }
        }

        /// <summary>
        /// Gets all tasks for the user, grouped by folders.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>Grouped tasks by folders.</returns>
        /// <response code="200">Returns grouped tasks.</response>
        /// <response code="500">If an error occurs.</response>
        [Authorize]
        [HttpGet("grouped/user/{userId}")]
        public async Task<IActionResult> GetGroupedByFolders(string userId)
        {
            try
            {
                var result = await _taskService.GetGroupedTasksByFoldersAsync(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get grouped tasks for user with ID {UserId}", userId);
                return StatusCode(500, $"Failed to get grouped tasks for user with ID {userId}");
            }
        }

        /// <summary>
        /// Marks a repeatable task as checked in for the specified date.
        /// Adds date to <c>CheckedInDays</c> tasks, if it matches with one from <c>RepeatDays</c>.
        /// </summary>
        /// <param name="taskId">The task ID.</param>
        /// <param name="date">The date to check in (e.g., <c>DateTime.Today</c>).</param>
        /// <returns>The updated repeatable task with the new check-in date.</returns>
        /// <response code="200">Returns the updated repeatable task.</response>
        /// <response code="400">If the task ID is missing, or the date is invalid/not allowed.</response>
        /// <response code="404">If the task is not found or is not a repeatable task.</response>
        /// <response code="500">If an error occurs.</response>
        [Authorize]
        [HttpPost("{taskId}/checkin")]
        public async Task<ActionResult<TaskRepeatableDto>> MarkRepeatableTaskCheckedIn(
            string taskId,
            [FromBody] RepeatableSubtaskCheckinDto checkinDto)
        {
            if (string.IsNullOrWhiteSpace(taskId))
                return BadRequest("Task ID is required.");

            try
            {
                var result = await _taskService.MarkRepeatableTaskCheckedInAsync(taskId, checkinDto.Date);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex, "Task not found or not repeatable. TaskId={TaskId}", taskId);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to check in repeatable task with ID {TaskId}", taskId);
                return StatusCode(500, $"Failed to check in repeatable task with ID {taskId}");
            }
        }

    }
}
