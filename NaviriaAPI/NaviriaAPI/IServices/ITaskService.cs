using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.DTOs.FeaturesDTOs;
using NaviriaAPI.DTOs.TaskDtos;

namespace NaviriaAPI.IServices
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskDto>> GetAllByUserAsync(string userId);
        Task<TaskDto?> GetByIdAsync(string id);
        Task<TaskDto> CreateAsync(TaskCreateDto dto);
        Task<bool> UpdateAsync(string id, TaskUpdateDto dto);
        Task<bool> DeleteAsync(string id);
        Task<IEnumerable<FolderWithTasksDto>> GetGroupedTasksByFoldersAsync(string userId);

    }

}
