﻿using System.Text.Json;
using NaviriaAPI.Entities.EmbeddedEntities.Subtasks;
using NaviriaAPI.Entities;
using NaviriaAPI.DTOs.Task.Create;
using NaviriaAPI.DTOs.Task.Subtask.View;
using NaviriaAPI.DTOs.Task.Subtask.Update;
using NaviriaAPI.DTOs.Task.View;
using NaviriaAPI.DTOs.Task.Update;
using NaviriaAPI.DTOs.Task.Subtask.Create;

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