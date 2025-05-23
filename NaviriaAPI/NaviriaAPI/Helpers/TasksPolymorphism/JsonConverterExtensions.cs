using System.Text.Json;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.DTOs.TaskDtos;
using NaviriaAPI.Entities.EmbeddedEntities.Subtasks;
using NaviriaAPI.Entities;

namespace NaviriaAPI.Helpers.TasksPolymorphism
{

    public static class JsonConverterExtensions
    {
        public static void AddPolymorphicSubtaskConverters(this JsonSerializerOptions options)
        {
            options.Converters.Add(
                new PolymorphicJsonConverter<SubtaskCreateDtoBase>("subtask_type", SubtaskTypeMap.CreateMap));
            options.Converters.Add(
                new PolymorphicJsonConverter<SubtaskUpdateDtoBase>("subtask_type", SubtaskTypeMap.UpdateMap));
            options.Converters.Add(
                new PolymorphicJsonConverter<SubtaskDtoBase>("subtask_type", SubtaskTypeMap.ReadMap));
            options.Converters.Add(
                new PolymorphicJsonConverter<SubtaskBase>("subtask_type", SubtaskTypeMap.EntityMap));
        }

        public static void AddPolymorphicTaskConverters(this JsonSerializerOptions options)
        {
            options.Converters.Add(
                new PolymorphicJsonConverter<TaskCreateDto>("type", TaskTypeMap.CreateMap));
            options.Converters.Add(
                new PolymorphicJsonConverter<TaskUpdateDto>("type", TaskTypeMap.UpdateMap));
            options.Converters.Add(
                new PolymorphicJsonConverter<TaskDto>("type", TaskTypeMap.ReadMap));
            options.Converters.Add(
                new PolymorphicJsonConverter<TaskEntity>("type", TaskTypeMap.EntityMap));
        }
    }
}