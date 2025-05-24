using NaviriaAPI.DTOs.TaskDtos;
using NaviriaAPI.Entities.EmbeddedEntities.TaskTypes;
using NaviriaAPI.DTOs.Task.Create;
using NaviriaAPI.DTOs.Task.Update;
using NaviriaAPI.DTOs.Task.View;

namespace NaviriaAPI.Helpers.TasksPolymorphism
{
    public static class TaskTypeMap
    {
        public static Dictionary<string, Type> CreateMap => new()
{
    { "standard", typeof(TaskStandartCreateDto) },
    { "scale", typeof(TaskScaleCreateDto) },
    { "repeatable", typeof(TaskRepeatableCreateDto) },
    { "with_subtasks", typeof(TaskWithSubtasksCreateDto) }
};
        public static Dictionary<string, Type> UpdateMap => new()
{
    { "standard", typeof(TaskStandartUpdateDto) },
    { "scale", typeof(TaskScaleUpdateDto) },
    { "repeatable", typeof(TaskRepeatableUpdateDto) },
    { "with_subtasks", typeof(TaskWithSubtasksUpdateDto) }
};
        public static Dictionary<string, Type> ReadMap => new()
{
    { "standard", typeof(TaskStandartDto) },
    { "scale", typeof(TaskScaleDto) },
    { "repeatable", typeof(TaskRepeatableDto) },
    { "with_subtasks", typeof(TaskWithSubtasksDto) }
};
        public static Dictionary<string, Type> EntityMap => new()
{
    { "standard", typeof(TaskStandart) },
    { "scale", typeof(TaskScale) },
    { "repeatable", typeof(TaskRepeatable) },
    { "with_subtasks", typeof(TaskWithSubtasks) }
};

    }
}
