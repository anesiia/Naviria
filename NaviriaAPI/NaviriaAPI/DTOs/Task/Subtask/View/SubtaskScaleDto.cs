namespace NaviriaAPI.DTOs.Task.Subtask.View
{
    public class SubtaskScaleDto : SubtaskDtoBase
    {
        public string Unit { get; set; } = string.Empty;
        public double CurrentValue { get; set; }
        public double TargetValue { get; set; }

        public SubtaskScaleDto()
        {
            Type = "scale";
        }
    }
}
