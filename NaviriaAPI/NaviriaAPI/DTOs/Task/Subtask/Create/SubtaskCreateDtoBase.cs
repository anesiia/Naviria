namespace NaviriaAPI.DTOs.Task.Subtask.Create
{
    public abstract class SubtaskCreateDtoBase
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }

}
