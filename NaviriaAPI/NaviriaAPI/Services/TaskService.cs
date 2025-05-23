using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices;
using NaviriaAPI.Mappings;
using NaviriaAPI.DTOs.FeaturesDTOs;
using NaviriaAPI.Exceptions;
using NaviriaAPI.IServices.ISecurityService;
using NaviriaAPI.IServices.IUserServices;
using NaviriaAPI.IServices.IGamificationLogic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Driver;
using NaviriaAPI.DTOs.Task.Create;
using NaviriaAPI.DTOs.Task.View;

namespace NaviriaAPI.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IFolderRepository _folderRepository;
        private readonly ILogger<TaskService> _logger;
        private readonly IUserService _userService;
        private readonly IMessageSecurityService _messageSecurityService;
        private readonly ITaskRewardService _taskRewardService;

        public TaskService(
            ITaskRepository taskRepository,
            ILogger<TaskService> logger,
            IFolderRepository folderRepository,
            IUserService userService,
            IMessageSecurityService messageSecurityService,
            ITaskRewardService taskRewardService)
        {
            _taskRepository = taskRepository;
            _logger = logger;
            _folderRepository = folderRepository;
            _userService = userService;
            _messageSecurityService = messageSecurityService;
            _taskRewardService = taskRewardService;
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public async Task<TaskDto?> GetByIdAsync(string id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
            {
                throw new NotFoundException($"Task with ID {id} not found.");
            }
            return TaskMapper.ToDto(task);
        }

        /// <inheritdoc />
        public async Task<TaskDto> CreateAsync(TaskCreateDto dto)
        {
            if (!await _userService.UserExistsAsync(dto.UserId))
            {
                throw new NotFoundException($"User with ID {dto.UserId} not found.");
            }

            // Validate title and description for security
            _messageSecurityService.Validate(dto.UserId, dto.Title);
            _messageSecurityService.Validate(dto.UserId, dto.Description);

            if (dto.Tags?.Count > 10)
                throw new ValidationException("A task can have no more than 10 tags.");

            var entity = TaskMapper.ToEntity(dto);
            await _taskRepository.CreateAsync(entity);

            return TaskMapper.ToDto(entity);
        }

        /// <inheritdoc />
        public async Task<bool> UpdateAsync(string id, TaskUpdateDto dto)
        {
            var existing = await _taskRepository.GetByIdAsync(id)
                ?? throw new NotFoundException($"Task with ID {id} not found.");

            ValidateTaskFields(existing.UserId, dto);

            if (dto.Tags?.Count > 10)
                throw new ValidationException("A task can have no more than 10 tags.");

            var prevStatus = existing.Status;
            var newStatus = dto.Status;
            bool isStatusChanged = prevStatus != newStatus;

            if (isStatusChanged)
            {

                var user = await _userService.GetByIdAsync(existing.UserId);

                if (user == null)
                    throw new NotFoundException($"User with ID {existing.UserId} not found.");

                await _taskRewardService.GrantTaskCompletionRewardsAsync(existing, user, prevStatus, newStatus);
            }

            existing = TaskMapper.UpdateEntity(existing, dto);

            var result = await _taskRepository.UpdateAsync(existing);
            return result;
        }


        /// <inheritdoc />
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

        /// <inheritdoc />
        public async Task<IEnumerable<FolderWithTasksDto>> GetGroupedTasksByFoldersAsync(string userId)
        {
            _logger.LogInformation("Grouping tasks by folders for user {UserId}", userId);

            if (!await _userService.UserExistsAsync(userId))
            {
                throw new NotFoundException($"User with ID {userId} not found.");
            }

            var tasks = (await _taskRepository.GetAllByUserAsync(userId)).ToList();
            var folders = (await _folderRepository.GetAllByUserIdAsync(userId)).ToList();

            var grouped = folders.Select(folder =>
        new FolderWithTasksDto
        {
            FolderId = folder.Id,
            FolderName = folder.Name,
            Tasks = tasks
                .Where(t => t.FolderId == folder.Id)
                .Select(TaskMapper.ToDto)
                .ToList()
        });

            return grouped;
        }

        /// <inheritdoc />
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

        private void ValidateTaskFields(string userId, TaskUpdateDto dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.Title))
                _messageSecurityService.Validate(userId, dto.Title);
            if (!string.IsNullOrWhiteSpace(dto.Description))
                _messageSecurityService.Validate(userId, dto.Description);
        }

    }
}
