namespace NaviriaAPI.DTOs.TaskDtos
{
    public class TaskRepeatableDto : TaskDto
    {
        public List<DayOfWeek> RepeatDays { get; set; } = new();
        public List<DateTime> CheckedInDays { get; set; } = new();

        public TaskRepeatableDto()
        {
            Type = "repeatable";
        }
    }
}
