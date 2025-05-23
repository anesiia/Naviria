using NaviriaAPI.DTOs.UpdateDTOs;

namespace NaviriaAPI.DTOs.UpdateDTOs
{
    public class ScaleSubtaskUpdateDto : SubtaskUpdateDtoBase
    {
        public string Unit { get; set; } = string.Empty;
        public double CurrentValue { get; set; }
        public double TargetValue { get; set; }

        public ScaleSubtaskUpdateDto()
        {
            Type = "scale";
        }

    }
}
