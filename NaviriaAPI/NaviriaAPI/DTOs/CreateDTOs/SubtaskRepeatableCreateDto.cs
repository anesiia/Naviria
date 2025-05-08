namespace NaviriaAPI.DTOs.CreateDTOs
{
    public class SubtaskRepeatableCreateDto : SubtaskCreateDtoBase
    {
        public List<DayOfWeek> RepeatDays { get; set; } = new();
        public string Type { get; set; } = "repeatable";
    }
}