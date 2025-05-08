using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices;
using NaviriaAPI.Mappings;
using NaviriaAPI.DTOs.FeaturesDTOs;
using NaviriaAPI.Repositories;
using NaviriaAPI.DTOs.TaskDtos;

namespace NaviriaAPI.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IFolderRepository _folerRepository;
        private readonly ILogger<TaskService> _logger;

        public TaskService(ITaskRepository taskRepository, ILogger<TaskService> logger, IFolderRepository folerRepository)
        {
            _taskRepository = taskRepository;
            _logger = logger;
            _folerRepository = folerRepository;
        }

        public async Task<IEnumerable<TaskDto>> GetAllByUserAsync(string userId)
        {
            var tasks = await _taskRepository.GetAllByUserAsync(userId);
            return tasks.Select(TaskMapper.ToDto);
        }

        public async Task<TaskDto?> GetByIdAsync(string id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            return task == null ? null : TaskMapper.ToDto(task);
        }

        public async Task<TaskDto> CreateAsync(TaskCreateDto dto)
        {
            var entity = TaskMapper.ToEntity(dto);
            await _taskRepository.CreateAsync(entity);
            return TaskMapper.ToDto(entity);
        }

        public async Task<bool> UpdateAsync(string id, TaskUpdateDto dto)
        {
            var existing = await _taskRepository.GetByIdAsync(id);
            if (existing == null) return false;

            // Мапимо оновлені поля
            existing = TaskMapper.UpdateEntity(existing, dto);

            return await _taskRepository.UpdateAsync(existing);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _taskRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<FolderWithTasksDto>> GetGroupedTasksByFoldersAsync(string userId)
        {
            var tasks = await _taskRepository.GetAllByUserAsync(userId);
            var folders = await _folerRepository.GetAllByUserIdAsync(userId); // потрібен IFolderRepository

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

    }


}
