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
                new PolymorphicJsonConverter<SubtaskCreateDtoBase>("type", SubtaskTypeMap.CreateMap));
            options.Converters.Add(
                new PolymorphicJsonConverter<SubtaskUpdateDtoBase>("type", SubtaskTypeMap.UpdateMap));
            options.Converters.Add(
                new PolymorphicJsonConverter<SubtaskDtoBase>("type", SubtaskTypeMap.ReadMap));
            options.Converters.Add(
                new PolymorphicJsonConverter<SubtaskBase>("type", SubtaskTypeMap.EntityMap));
        }
    }
}