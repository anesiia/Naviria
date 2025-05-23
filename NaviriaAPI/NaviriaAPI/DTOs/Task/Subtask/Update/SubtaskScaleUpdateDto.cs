using NaviriaAPI.DTOs.Task.Subtask.Update;

namespace NaviriaAPI.DTOs.UpdateDTOs
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
