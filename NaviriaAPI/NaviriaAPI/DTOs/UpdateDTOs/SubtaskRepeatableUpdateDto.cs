using System;

namespace NaviriaAPI.DTOs.UpdateDTOs
{
    public class SubtaskRepeatableUpdateDto
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<DayOfWeek> RepeatDays { get; set; } = new();
        public string Type { get; set; } = "repeatable";
    }
}