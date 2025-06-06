using NaviriaAPI.DTOs.Task.Subtask.View;

namespace NaviriaAPI.DTOs.Task.View
{
    public class TaskWithSubtasksDto : TaskDto
    {
        public List<SubtaskDtoBase> Subtasks { get; set; } = new();
        public TaskWithSubtasksDto()
        {
            Type = "with_subtasks";
        }
    }
}
