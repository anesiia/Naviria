using MongoDB.Bson;
using NaviriaAPI.DTOs.TaskDtos;
using NaviriaAPI.Entities.EmbeddedEntities.Subtasks;
using NaviriaAPI.DTOs.Task.Subtask.Create;
using NaviriaAPI.DTOs.Task.Subtask.View;
using NaviriaAPI.DTOs.Task.Subtask.Update;

namespace NaviriaAPI.Mappings
{
    public static class SubtaskMapper
    {
        public static SubtaskBase FromCreateDto(object dto)
        {
            return dto switch
            {
                SubtaskStandardCreateDto s => new SubtaskStandard
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Title = s.Title,
                    Description = s.Description,
                    IsCompleted = s.IsCompleted,
                    Type = s.Type
                },

                SubtaskRepeatableCreateDto r => new SubtaskRepeatable
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Title = r.Title,
                    Description = r.Description,
                    RepeatDays = r.RepeatDays,
                    Type = r.Type
                },

                SubtaskScaleCreateDto sc => new ScaleSubtask
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Title = sc.Title,
                    Description = sc.Description,
                    Unit = sc.Unit,
                    CurrentValue = sc.CurrentValue,
                    TargetValue = sc.TargetValue,
                    Type = sc.Type
                },

                _ => throw new ArgumentOutOfRangeException(nameof(dto), dto, "Unsupported subtask create DTO type.")
            };
        }

        public static SubtaskBase FromUpdateDto(string id, object dto)
        {
            return dto switch
            {
                SubtaskStandardUpdateDto s => new SubtaskStandard
                {
                    Id = id,
                    Title = s.Title,
                    Description = s.Description,
                    IsCompleted = s.IsCompleted,
                    Type = s.Type
                },

                SubtaskRepeatableUpdateDto r => new SubtaskRepeatable
                {
                    Id = id,
                    Title = r.Title,
                    Description = r.Description,
                    RepeatDays = r.RepeatDays,
                    Type = r.Type
                },

                SubtaskScaleUpdateDto sc => new ScaleSubtask
                {
                    Id = id,
                    Title = sc.Title,
                    Description = sc.Description,
                    Unit = sc.Unit,
                    CurrentValue = sc.CurrentValue,
                    TargetValue = sc.TargetValue,
                    Type = sc.Type
                },

                _ => throw new ArgumentOutOfRangeException(nameof(dto), dto, "Unsupported subtask update DTO type.")
            };
        }

        public static SubtaskDtoBase ToDto(SubtaskBase subtask)
        {
            return subtask switch
            {
                SubtaskStandard s => new SubtaskStandardDto
                {
                    Id = s.Id,
                    Title = s.Title,
                    Description = s.Description,
                    IsCompleted = s.IsCompleted,
                    Type = s.Type
                },

                SubtaskRepeatable r => new SubtaskRepeatableDto
                {
                    Id = r.Id,
                    Title = r.Title,
                    Description = r.Description,
                    RepeatDays = r.RepeatDays,
                    Type = r.Type
                },

                ScaleSubtask sc => new SubtaskScaleDto
                {
                    Id = sc.Id,
                    Title = sc.Title,
                    Description = sc.Description,
                    Unit = sc.Unit,
                    CurrentValue = sc.CurrentValue,
                    TargetValue = sc.TargetValue,
                    Type = sc.Type
                },

                _ => throw new NotSupportedException($"Unknown subtask entity type: {subtask.GetType().Name}")
            };
        }
    }
}
