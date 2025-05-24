namespace NaviriaAPI.DTOs.Task.Subtask.Update
{
    public class SubtaskRepeatableUpdateDto : SubtaskUpdateDtoBase
    {
        public List<DayOfWeek> RepeatDays { get; set; } = new();
        public List<DateTime> CheckedInDays { get; set; } = new();

        public SubtaskRepeatableUpdateDto()
        {
            Type = "repeatable";
        }
    }
}