namespace NaviriaAPI.DTOs.Task.Create
{
    public class TaskScaleCreateDto : TaskCreateDto
    {
        public string Unit { get; set; } = string.Empty;
        public double CurrentValue { get; set; }
        public double TargetValue { get; set; }

        public TaskScaleCreateDto()
        {
            Type = "scale";
        }
    }
}
