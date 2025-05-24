using NaviriaAPI.IRepositories;
using NaviriaAPI.DTOs.Task.Subtask.Create;
using NaviriaAPI.Mappings;
using NaviriaAPI.IServices;
using NaviriaAPI.DTOs.Task.Subtask.Update;
using NaviriaAPI.Entities.EmbeddedEntities.Subtasks;
using NaviriaAPI.Exceptions;
using System.ComponentModel.DataAnnotations;
using NaviriaAPI.DTOs.Task.Subtask.View;

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

        /// <inheritdoc />
        public async Task<SubtaskRepeatableDto> MarkRepeatableSubtaskCheckedInAsync(string taskId, string subtaskId, DateTime date)
        {
            var task = await _taskRepository.GetByIdAsync(taskId)
                ?? throw new NotFoundException($"Task with id {taskId} not found");

            var subtask = task.Subtasks.FirstOrDefault(s => s.Id == subtaskId)
                as SubtaskRepeatable
                ?? throw new ValidationException("Subtask is not of type 'repeatable'");

            if (!subtask.RepeatDays.Contains(date.DayOfWeek))
                throw new ValidationException($"Date {date.ToShortDateString()} is not allowed for this repeatable subtask");

            if (!subtask.CheckedInDays.Any(d => d.Date == date.Date))
            {
                subtask.CheckedInDays.Add(date.Date);
                await _taskRepository.UpdateAsync(task);
            }

            return (SubtaskRepeatableDto)SubtaskMapper.ToDto(subtask);
        }


    }
}
