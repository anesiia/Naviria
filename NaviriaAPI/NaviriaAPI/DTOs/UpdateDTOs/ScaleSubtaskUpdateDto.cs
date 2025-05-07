namespace NaviriaAPI.DTOs.UpdateDTOs
{
    public class ScaleSubtaskUpdateDto
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public double CurrentValue { get; set; }
        public double TargetValue { get; set; }
        public string Type { get; set; } = "scale";
    }
}