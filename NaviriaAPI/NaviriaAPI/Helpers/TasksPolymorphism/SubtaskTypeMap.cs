using NaviriaAPI.Entities.EmbeddedEntities.Subtasks;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.DTOs.TaskDtos;

namespace NaviriaAPI.Helpers
{
    public static class SubtaskTypeMap
    {
        public static Dictionary<string, Type> CreateMap => new()
        {
            { "standard", typeof(SubtaskStandardCreateDto) },
            { "repeatable", typeof(SubtaskRepeatableCreateDto) },
            { "scale", typeof(ScaleSubtaskCreateDto) }
        };

        public static Dictionary<string, Type> UpdateMap => new()
        {
            { "standard", typeof(SubtaskStandardUpdateDto) },
            { "repeatable", typeof(SubtaskRepeatableUpdateDto) },
            { "scale", typeof(ScaleSubtaskUpdateDto) }
        };

        public static Dictionary<string, Type> ReadMap => new()
        {
            { "standard", typeof(SubtaskStandardDto) },
            { "repeatable", typeof(SubtaskRepeatableDto) },
            { "scale", typeof(ScaleSubtaskDto) }
        };

        public static Dictionary<string, Type> EntityMap => new()
        {
            { "standard", typeof(SubtaskStandard) },
            { "repeatable", typeof(SubtaskRepeatable) },
            { "scale", typeof(ScaleSubtask) }
        };
    }
}