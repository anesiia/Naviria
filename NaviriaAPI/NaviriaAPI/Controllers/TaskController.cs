using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NaviriaAPI.Services;

namespace NaviriaAPI.Controllers
{
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

        [HttpPost]
        [AllowAnonymous]
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


        [HttpPut("{id}")]
        [AllowAnonymous]
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
    }
}
