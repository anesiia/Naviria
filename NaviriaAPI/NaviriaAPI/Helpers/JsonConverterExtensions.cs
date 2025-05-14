using System.Text.Json;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.DTOs.TaskDtos;
using NaviriaAPI.Entities.EmbeddedEntities.Subtasks;

namespace NaviriaAPI.Helpers
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
    }
}