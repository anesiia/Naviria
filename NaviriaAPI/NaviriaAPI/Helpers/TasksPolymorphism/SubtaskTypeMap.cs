using NaviriaAPI.Entities.EmbeddedEntities.Subtasks;
using NaviriaAPI.DTOs.TaskDtos;
using NaviriaAPI.DTOs.Task.Subtask.Create;
using NaviriaAPI.DTOs.Task.Subtask.View;
using NaviriaAPI.DTOs.Task.Subtask.Update;

namespace NaviriaAPI.Helpers
{
    public static class SubtaskTypeMap
    {
        public static Dictionary<string, Type> CreateMap => new()
        {
            { "standard", typeof(SubtaskStandardCreateDto) },
            { "repeatable", typeof(SubtaskRepeatableCreateDto) },
            { "scale", typeof(SubtaskScaleCreateDto) }
        };

        public static Dictionary<string, Type> UpdateMap => new()
        {
            { "standard", typeof(SubtaskStandardUpdateDto) },
            { "repeatable", typeof(SubtaskRepeatableUpdateDto) },
            { "scale", typeof(SubtaskScaleUpdateDto) }
        };

        public static Dictionary<string, Type> ReadMap => new()
        {
            { "standard", typeof(SubtaskStandardDto) },
            { "repeatable", typeof(SubtaskRepeatableDto) },
            { "scale", typeof(SubtaskScaleDto) }
        };

        public static Dictionary<string, Type> EntityMap => new()
        {
            { "standard", typeof(SubtaskStandard) },
            { "repeatable", typeof(SubtaskRepeatable) },
            { "scale", typeof(ScaleSubtask) }
        };
    }
}