using NaviriaAPI.DTOs.UpdateDTOs;

namespace NaviriaAPI.DTOs.Task.Update
{
    public class TaskScaleUpdateDto : TaskUpdateDto
    {
        public string Unit { get; set; } = string.Empty;
        public double CurrentValue { get; set; }
        public double TargetValue { get; set; }

        public TaskScaleUpdateDto()
        {
            Type = "scale";
        }
    }
}
