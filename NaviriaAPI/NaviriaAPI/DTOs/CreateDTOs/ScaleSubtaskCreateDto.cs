namespace NaviriaAPI.DTOs.CreateDTOs
{
    public class ScaleSubtaskCreateDto : SubtaskCreateDtoBase
    {
        public string Unit { get; set; } = string.Empty;
        public double CurrentValue { get; set; }
        public double TargetValue { get; set; }

        public ScaleSubtaskCreateDto()
        {
            Type = "scale";
        }
    }

}