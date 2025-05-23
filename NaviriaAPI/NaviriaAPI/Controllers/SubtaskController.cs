using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.IServices;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using Microsoft.AspNetCore.Authorization;

namespace NaviriaAPI.Controllers
{
    /// <summary>
    /// Controller for managing subtasks within a task.
    /// </summary>
    [ApiController]
    [Route("api/tasks/{taskId}/subtasks")]
    public class SubtaskController : ControllerBase
    {
        private readonly ISubtaskService _subtaskService;
        private readonly ILogger<SubtaskController> _logger;

        public SubtaskController(
            ISubtaskService subtaskService,
            ILogger<SubtaskController> logger)
        {
            _subtaskService = subtaskService;
            _logger = logger;
        }

        /// <summary>
        /// Adds a new subtask to the specified task.
        /// </summary>
        /// <param name="taskId">The ID of the task to add the subtask to.</param>
        /// <param name="subtaskDto">The subtask creation DTO.</param>
        /// <returns>Status indicating the result of the operation.</returns>
        /// <response code="200">Subtask added successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="404">If the task is not found.</response>
        /// <response code="500">If an internal error occurs.</response>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddSubtask(string taskId, [FromBody] SubtaskCreateDtoBase subtaskDto)
        {
            if (string.IsNullOrWhiteSpace(taskId))
                return BadRequest("Task ID is required.");

            if (subtaskDto == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _subtaskService.AddSubtaskAsync(taskId, subtaskDto);
                return result ? Ok() : NotFound("Task not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add subtask to task {TaskId}", taskId);
                return StatusCode(500, $"Failed to add subtask to task {taskId}");
            }
        }

        /// <summary>
        /// Updates a subtask within the specified task.
        /// </summary>
        /// <param name="taskId">The ID of the task containing the subtask.</param>
        /// <param name="subtaskId">The ID of the subtask to update.</param>
        /// <param name="subtaskDto">The subtask update DTO.</param>
        /// <returns>Status indicating the result of the operation.</returns>
        /// <response code="200">Subtask updated successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="404">If the task or subtask is not found.</response>
        /// <response code="500">If an internal error occurs.</response>
        [HttpPut("{subtaskId}")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateSubtask(
            string taskId,
            string subtaskId,
            [FromBody] SubtaskUpdateDtoBase subtaskDto)
        {
            if (string.IsNullOrWhiteSpace(taskId) || string.IsNullOrWhiteSpace(subtaskId))
                return BadRequest("Task ID and subtask ID are required.");

            if (subtaskDto == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _subtaskService.UpdateSubtaskAsync(taskId, subtaskId, subtaskDto);
                return result ? Ok() : NotFound("Task or subtask not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update subtask {SubtaskId} in task {TaskId}", subtaskId, taskId);
                return StatusCode(500, $"Failed to update subtask {subtaskId} in task {taskId}");
            }
        }

        /// <summary>
        /// Removes a subtask from the specified task.
        /// </summary>
        /// <param name="taskId">The ID of the task containing the subtask.</param>
        /// <param name="subtaskId">The ID of the subtask to remove.</param>
        /// <returns>Status indicating the result of the operation.</returns>
        /// <response code="200">Subtask removed successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="404">If the task or subtask is not found.</response>
        /// <response code="500">If an internal error occurs.</response>
        [HttpDelete("{subtaskId}")]
        [AllowAnonymous]
        public async Task<IActionResult> RemoveSubtask(string taskId, string subtaskId)
        {
            if (string.IsNullOrWhiteSpace(taskId) || string.IsNullOrWhiteSpace(subtaskId))
                return BadRequest("Task ID and subtask ID are required.");

            try
            {
                var result = await _subtaskService.RemoveSubtaskAsync(taskId, subtaskId);
                return result ? Ok() : NotFound("Task or subtask not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to remove subtask {SubtaskId} from task {TaskId}", subtaskId, taskId);
                return StatusCode(500, $"Failed to remove subtask {subtaskId} from task {taskId}");
            }
        }
    }
}
