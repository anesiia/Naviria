using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices;
using NaviriaAPI.Mappings;
using NaviriaAPI.DTOs.FeaturesDTOs;
using NaviriaAPI.DTOs.TaskDtos;
using NaviriaAPI.Exceptions;
using NaviriaAPI.IServices.ISecurityService;
using NaviriaAPI.IServices.IUserServices;

namespace NaviriaAPI.Services
{
    /// <summary>
    /// Service for managing user tasks and task-related operations.
    /// </summary>
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IFolderRepository _folderRepository;
        private readonly ILogger<TaskService> _logger;
        private readonly IUserService _userService;
        private readonly IMessageSecurityService _messageSecurityService;

        public TaskService(
            ITaskRepository taskRepository,
            ILogger<TaskService> logger,
            IFolderRepository folderRepository,
            IUserService userService,
            IMessageSecurityService messageSecurityService)
        {
            _taskRepository = taskRepository;
            _logger = logger;
            _folderRepository = folderRepository;
            _userService = userService;
            _messageSecurityService = messageSecurityService;
        }

        /// <summary>
        /// Gets all tasks for the specified user.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <returns>A list of TaskDto objects.</returns>
        /// <exception cref="NotFoundException">Thrown if the user does not exist or has no tasks.</exception>
        public async Task<IEnumerable<TaskDto>> GetAllByUserAsync(string userId)
        {
            if (!await _userService.UserExistsAsync(userId))
            {
                throw new NotFoundException($"User with ID {userId} not found.");
            }

            var tasks = (await _taskRepository.GetAllByUserAsync(userId)).ToList();
            if (!tasks.Any())
            {
                throw new NotFoundException($"User with ID {userId} has no tasks.");
            }

            return tasks.Select(TaskMapper.ToDto);
        }

        /// <summary>
        /// Gets a task by its ID.
        /// </summary>
        /// <param name="id">Task ID.</param>
        /// <returns>The TaskDto object.</returns>
        /// <exception cref="NotFoundException">Thrown if the task does not exist.</exception>
        public async Task<TaskDto?> GetByIdAsync(string id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
            {
                throw new NotFoundException($"Task with ID {id} not found.");
            }
            return TaskMapper.ToDto(task);
        }

        /// <summary>
        /// Creates a new task.
        /// </summary>
        /// <param name="dto">Task creation data transfer object.</param>
        /// <returns>The created TaskDto.</returns>
        /// <exception cref="NotFoundException">Thrown if the user does not exist.</exception>
        /// <exception cref="SuspiciousMessageException">Thrown if the title or description contains forbidden content.</exception>
        public async Task<TaskDto> CreateAsync(TaskCreateDto dto)
        {
            if (!await _userService.UserExistsAsync(dto.UserId))
            {
                throw new NotFoundException($"User with ID {dto.UserId} not found.");
            }

            // Validate title and description for security
            _messageSecurityService.Validate(dto.UserId, dto.Title);
            _messageSecurityService.Validate(dto.UserId, dto.Description);

            var entity = TaskMapper.ToEntity(dto);
            await _taskRepository.CreateAsync(entity);

            return TaskMapper.ToDto(entity);
        }

        /// <summary>
        /// Updates an existing task.
        /// </summary>
        /// <param name="id">Task ID.</param>
        /// <param name="dto">Task update data transfer object.</param>
        /// <returns>True if updated successfully, otherwise false.</returns>
        /// <exception cref="NotFoundException">Thrown if the task does not exist.</exception>
        /// <exception cref="SuspiciousMessageException">Thrown if the title or description contains forbidden content.</exception>
        public async Task<bool> UpdateAsync(string id, TaskUpdateDto dto)
        {
            var existing = await _taskRepository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new NotFoundException($"Task with ID {id} not found.");
            }

            // Validate updated title/description if provided
            if (!string.IsNullOrWhiteSpace(dto.Title))
                _messageSecurityService.Validate(existing.UserId, dto.Title);
            if (!string.IsNullOrWhiteSpace(dto.Description))
                _messageSecurityService.Validate(existing.UserId, dto.Description);

            existing = TaskMapper.UpdateEntity(existing, dto);

            var result = await _taskRepository.UpdateAsync(existing);
            return result;
        }

        /// <summary>
        /// Deletes a task by its ID.
        /// </summary>
        /// <param name="id">Task ID.</param>
        /// <returns>True if deleted successfully, otherwise false.</returns>
        /// <exception cref="NotFoundException">Thrown if the task does not exist.</exception>
        public async Task<bool> DeleteAsync(string id)
        {
            var existing = await _taskRepository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new NotFoundException($"Task with ID {id} not found.");
            }

            var result = await _taskRepository.DeleteAsync(id);
            return result;
        }

        /// <summary>
        /// Gets all tasks for a user, grouped by folders.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <returns>A collection of FolderWithTasksDto with grouped tasks.</returns>
        /// <exception cref="NotFoundException">Thrown if the user does not exist or has no tasks.</exception>
        public async Task<IEnumerable<FolderWithTasksDto>> GetGroupedTasksByFoldersAsync(string userId)
        {
            _logger.LogInformation("Grouping tasks by folders for user {UserId}", userId);

            if (!await _userService.UserExistsAsync(userId))
            {
                throw new NotFoundException($"User with ID {userId} not found.");
            }

            var tasks = (await _taskRepository.GetAllByUserAsync(userId)).ToList();
            var folders = (await _folderRepository.GetAllByUserIdAsync(userId)).ToList();

            if (!tasks.Any())
            {
                throw new NotFoundException($"User with ID {userId} has no tasks.");
            }

            var grouped = tasks
                .GroupBy(t => t.FolderId)
                .Select(g =>
                {
                    var folderName = folders.FirstOrDefault(f => f.Id == g.Key)?.Name ?? "Unknown";
                    return new FolderWithTasksDto
                    {
                        FolderId = g.Key,
                        FolderName = folderName,
                        Tasks = g.Select(TaskMapper.ToDto).ToList()
                    };
                });

            return grouped;
        }

        /// <summary>
        /// Gets all tasks with a deadline on the specified date.
        /// </summary>
        /// <param name="deadlineDate">Deadline date.</param>
        /// <returns>A list of TaskDto with the specified deadline.</returns>
        /// <exception cref="NotFoundException">Thrown if no tasks are found with the specified deadline.</exception>
        public async Task<IEnumerable<TaskDto>> GetTasksWithDeadlineAsync(DateTime deadlineDate)
        {
            _logger.LogInformation("Getting tasks with deadline on {DeadlineDate}", deadlineDate.ToShortDateString());
            var tasks = (await _taskRepository.GetTasksWithDeadlineOnDateAsync(deadlineDate)).ToList();

            if (!tasks.Any())
            {
                throw new NotFoundException("No tasks found with the specified deadline.");
            }

            return tasks.Select(TaskMapper.ToDto);
        }
    }
}
