using NaviriaAPI.IRepositories;
using NaviriaAPI.Entities.EmbeddedEntities.Subtasks;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.Mappings;
using NaviriaAPI.IServices;

namespace NaviriaAPI.Services
{
    public class SubtaskService : ISubtaskService
    {
        private readonly ITaskRepository _taskRepository;

        public SubtaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        /// <inheritdoc />
        public async Task<bool> AddSubtaskAsync(string taskId, SubtaskCreateDtoBase subtaskDto)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null)
                return false;

            var subtask = SubtaskMapper.FromCreateDto(subtaskDto);
            task.Subtasks.Add(subtask);

            return await _taskRepository.UpdateAsync(task);
        }

        /// <inheritdoc />
        public async Task<bool> UpdateSubtaskAsync(string taskId, string subtaskId, SubtaskUpdateDtoBase subtaskDto)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null)
                return false;

            var index = task.Subtasks.FindIndex(s => s.Id == subtaskId);
            if (index == -1)
                return false;

            var updated = SubtaskMapper.FromUpdateDto(subtaskId, subtaskDto);
            updated.Id = subtaskId;
            task.Subtasks[index] = updated;

            return await _taskRepository.UpdateAsync(task);
        }

        /// <inheritdoc />
        public async Task<bool> RemoveSubtaskAsync(string taskId, string subtaskId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null)
                return false;

            var removed = task.Subtasks.RemoveAll(s => s.Id == subtaskId) > 0;
            if (!removed)
                return false;

            return await _taskRepository.UpdateAsync(task);
        }
    }
}
