namespace NaviriaAPI.DTOs.Task.Subtask.View
{
    public class SubtaskRepeatableDto : SubtaskDtoBase
    {
        public List<DayOfWeek> RepeatDays { get; set; } = new();
        public List<DateTime> CheckedInDays { get; set; } = new();

        public SubtaskRepeatableDto()
        {
            Type = "repeatable";
        }
    }
}
