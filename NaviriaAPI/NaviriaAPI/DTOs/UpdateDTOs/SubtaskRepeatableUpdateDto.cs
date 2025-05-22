using System;

namespace NaviriaAPI.DTOs.UpdateDTOs
{
    public class SubtaskRepeatableUpdateDto : SubtaskUpdateDtoBase
    {
        public List<DayOfWeek> RepeatDays { get; set; } = new();

        public SubtaskRepeatableUpdateDto()
        {
            Type = "repeatable";
        }
    }
}