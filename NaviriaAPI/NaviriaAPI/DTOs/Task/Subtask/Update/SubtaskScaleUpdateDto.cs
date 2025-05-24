namespace NaviriaAPI.DTOs.Task.Subtask.Update
{
    public class SubtaskScaleUpdateDto : SubtaskUpdateDtoBase
    {
        public string Unit { get; set; } = string.Empty;
        public double CurrentValue { get; set; }
        public double TargetValue { get; set; }

        public SubtaskScaleUpdateDto()
        {
            Type = "scale";
        }

    }
}
