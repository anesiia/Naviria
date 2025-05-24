namespace NaviriaAPI.DTOs.Task.Subtask.Create
{
    public class SubtaskScaleCreateDto : SubtaskCreateDtoBase
    {
        public string Unit { get; set; } = string.Empty;
        public double CurrentValue { get; set; }
        public double TargetValue { get; set; }

        public SubtaskScaleCreateDto()
        {
            Type = "scale";
        }
    }

}