namespace NaviriaAPI.DTOs.TaskDtos
{
    public class TaskScaleDto : TaskDto
    {
        public string Unit { get; set; } = string.Empty;
        public double CurrentValue { get; set; }
        public double TargetValue { get; set; }

        public TaskScaleDto()
        {
            Type = "scale";
        }
    }
}
