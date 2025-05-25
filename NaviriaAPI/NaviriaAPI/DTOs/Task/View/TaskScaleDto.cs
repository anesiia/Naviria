namespace NaviriaAPI.DTOs.Task.View
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
