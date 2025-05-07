namespace NaviriaAPI.DTOs.TaskDtos
{
    public class ScaleSubtaskDto : SubtaskDtoBase
    {
        public string Unit { get; set; } = string.Empty;
        public double CurrentValue { get; set; }
        public double TargetValue { get; set; }

        public ScaleSubtaskDto()
        {
            Type = "scale";
        }
    }
}
