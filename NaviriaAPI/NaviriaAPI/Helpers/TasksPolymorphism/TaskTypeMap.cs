// TaskTypeMap.cs
using NaviriaAPI.DTOs.TaskDtos;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.Entities.EmbeddedEntities.TaskTypes;

namespace NaviriaAPI.Helpers.TasksPolymorphism
{
    public static class TaskTypeMap
    {
        public static Dictionary<string, Type> CreateMap => new()
        {
            { "standard", typeof(TaskStandartCreateDto) },
            { "scale", typeof(TaskScaleCreateDto) },
            { "repeatable", typeof(TaskRepeatableCreateDto) }
        };
        public static Dictionary<string, Type> UpdateMap => new()
        {
            { "standard", typeof(TaskStandartUpdateDto) },
            { "scale", typeof(TaskScaleUpdateDto) },
            { "repeatable", typeof(TaskRepeatableUpdateDto) }
        };
        public static Dictionary<string, Type> ReadMap => new()
        {
            { "standard", typeof(TaskStandartDto) },
            { "scale", typeof(TaskScaleDto) },
            { "repeatable", typeof(TaskRepeatableDto) }
        };
        public static Dictionary<string, Type> EntityMap => new()
        {
            { "standard", typeof(TaskStandart) },
            { "scale", typeof(TaskScale) },
            { "repeatable", typeof(TaskRepeatable) }
        };
    }
}
