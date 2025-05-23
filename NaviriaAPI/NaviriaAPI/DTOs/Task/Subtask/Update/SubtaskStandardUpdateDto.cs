namespace NaviriaAPI.DTOs.Task.Subtask.Update
{
    public class SubtaskStandardUpdateDto : SubtaskUpdateDtoBase
    {
        public bool IsCompleted { get; set; }
        public SubtaskStandardUpdateDto()
        {
            Type = "standard";
        }
    }
}