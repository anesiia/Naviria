namespace NaviriaAPI.DTOs.UpdateDTOs
{
    public class SubtaskStandardUpdateDto
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public string Type { get; set; } = "standard";
    }
}