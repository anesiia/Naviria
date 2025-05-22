namespace NaviriaAPI.DTOs.TaskDtos
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
