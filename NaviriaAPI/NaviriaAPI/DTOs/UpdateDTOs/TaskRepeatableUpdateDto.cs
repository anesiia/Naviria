namespace NaviriaAPI.DTOs.UpdateDTOs
{
    public class TaskRepeatableUpdateDto : TaskUpdateDto
    {
        public List<DayOfWeek> RepeatDays { get; set; } = new();
        public List<DateTime> CheckedInDays { get; set; } = new();

        public TaskRepeatableUpdateDto()
        {
            Type = "repeatable";
        }
    }
}
