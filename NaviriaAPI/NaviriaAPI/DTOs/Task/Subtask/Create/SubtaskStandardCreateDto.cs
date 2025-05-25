namespace NaviriaAPI.DTOs.Task.Subtask.Create
{
    public class SubtaskStandardCreateDto : SubtaskCreateDtoBase
    {
        public bool IsCompleted { get; set; }
        public SubtaskStandardCreateDto()
        {
            Type = "standard";
        }
    }
}