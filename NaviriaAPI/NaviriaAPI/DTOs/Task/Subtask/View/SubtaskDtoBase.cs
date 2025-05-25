namespace NaviriaAPI.DTOs.Task.Subtask.View
{
    public abstract class SubtaskDtoBase
    {
        public string? Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }
}
