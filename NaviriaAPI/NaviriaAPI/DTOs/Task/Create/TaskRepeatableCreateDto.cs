using NaviriaAPI.DTOs.Task.Create;

namespace NaviriaAPI.DTOs.CreateDTOs
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
