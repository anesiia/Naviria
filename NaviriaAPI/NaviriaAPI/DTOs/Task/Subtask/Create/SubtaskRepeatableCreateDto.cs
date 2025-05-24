namespace NaviriaAPI.DTOs.Task.Subtask.Create
{
    public class SubtaskRepeatableCreateDto : SubtaskCreateDtoBase
    {
        public List<DayOfWeek> RepeatDays { get; set; } = new();
        public SubtaskRepeatableCreateDto()
        {
            Type = "repeatable";
        }
    }
}