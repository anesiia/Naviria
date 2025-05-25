using NaviriaAPI.DTOs.Task.Subtask.View;

namespace NaviriaAPI.DTOs.TaskDtos
{
    public class SubtaskStandardDto : SubtaskDtoBase
    {
        public bool IsCompleted { get; set; }

        public SubtaskStandardDto()
        {
            Type = "standard";
        }
    }
}
