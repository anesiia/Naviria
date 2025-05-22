namespace NaviriaAPI.DTOs.CreateDTOs
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