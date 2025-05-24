namespace NaviriaAPI.DTOs.Task.Create
{
    public class TaskRepeatableCreateDto : TaskCreateDto
    {
        public List<DayOfWeek> RepeatDays { get; set; } = new();
        public TaskRepeatableCreateDto()
        {
            Type = "repeatable";
        }
    }
}
