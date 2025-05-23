using MongoDB.Bson;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.TaskDtos;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.Entities.EmbeddedEntities.TaskTypes;
using NaviriaAPI.Entities;
using NaviriaAPI.Helpers;

namespace NaviriaAPI.Mappings
{
    public static class TaskMapper
    {
        public static TaskEntity ToEntity(TaskCreateDto dto)
        {
            switch (dto.Type)
            {
                case "repeatable":
                    var rep = (TaskRepeatableCreateDto)dto;
                    return new TaskRepeatable
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        UserId = rep.UserId,
                        FolderId = rep.FolderId,
                        Title = rep.Title,
                        Description = rep.Description,
                        CategoryId = rep.CategoryId,
                        Tags = rep.Tags,
                        IsDeadlineOn = rep.IsDeadlineOn,
                        Deadline = rep.Deadline,
                        IsShownProgressOnPage = rep.IsShownProgressOnPage,
                        IsNotificationsOn = rep.IsNotificationsOn,
                        NotificationDate = rep.NotificationDate,
                        Priority = rep.Priority,
                        Subtasks = rep.Subtasks.Select(SubtaskMapper.FromCreateDto).ToList(),
                        Status = CurrentTaskStatus.InProgress,
                        RepeatDays = rep.RepeatDays,
                        CheckedInDays = []
                    };
                case "scale":
                    var scale = (TaskScaleCreateDto)dto;
                    return new TaskScale
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        UserId = scale.UserId,
                        FolderId = scale.FolderId,
                        Title = scale.Title,
                        Description = scale.Description,
                        CategoryId = scale.CategoryId,
                        Tags = scale.Tags,
                        IsDeadlineOn = scale.IsDeadlineOn,
                        Deadline = scale.Deadline,
                        IsShownProgressOnPage = scale.IsShownProgressOnPage,
                        IsNotificationsOn = scale.IsNotificationsOn,
                        NotificationDate = scale.NotificationDate,
                        Priority = scale.Priority,
                        Subtasks = scale.Subtasks.Select(SubtaskMapper.FromCreateDto).ToList(),
                        Status = CurrentTaskStatus.InProgress,
                        Unit = scale.Unit,
                        CurrentValue = scale.CurrentValue,
                        TargetValue = scale.TargetValue
                    };
                default:
                    var std = (TaskStandartCreateDto)dto;
                    return new TaskStandart
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        UserId = std.UserId,
                        FolderId = std.FolderId,
                        Title = std.Title,
                        Description = std.Description,
                        CategoryId = std.CategoryId,
                        Tags = std.Tags,
                        IsDeadlineOn = std.IsDeadlineOn,
                        Deadline = std.Deadline,
                        IsShownProgressOnPage = std.IsShownProgressOnPage,
                        IsNotificationsOn = std.IsNotificationsOn,
                        NotificationDate = std.NotificationDate,
                        Priority = std.Priority,
                        Subtasks = std.Subtasks.Select(SubtaskMapper.FromCreateDto).ToList(),
                        Status = CurrentTaskStatus.InProgress
                    };
            }
        }

        public static TaskDto ToDto(TaskEntity entity)
        {
            switch (entity.Type)
            {
                case "repeatable":
                    var rep = (TaskRepeatable)entity;
                    return new TaskRepeatableDto
                    {
                        Id = rep.Id,
                        UserId = rep.UserId,
                        FolderId = rep.FolderId,
                        Title = rep.Title,
                        Description = rep.Description,
                        CategoryId = rep.CategoryId,
                        Tags = rep.Tags,
                        IsDeadlineOn = rep.IsDeadlineOn,
                        Deadline = rep.Deadline,
                        IsShownProgressOnPage = rep.IsShownProgressOnPage,
                        IsNotificationsOn = rep.IsNotificationsOn,
                        NotificationDate = rep.NotificationDate,
                        Priority = rep.Priority,
                        Subtasks = rep.Subtasks.Select(SubtaskMapper.ToDto).ToList(),
                        Status = rep.Status,
                        RepeatDays = rep.RepeatDays,
                        CheckedInDays = rep.CheckedInDays
                    };
                case "scale":
                    var scale = (TaskScale)entity;
                    return new TaskScaleDto
                    {
                        Id = scale.Id,
                        UserId = scale.UserId,
                        FolderId = scale.FolderId,
                        Title = scale.Title,
                        Description = scale.Description,
                        CategoryId = scale.CategoryId,
                        Tags = scale.Tags,
                        IsDeadlineOn = scale.IsDeadlineOn,
                        Deadline = scale.Deadline,
                        IsShownProgressOnPage = scale.IsShownProgressOnPage,
                        IsNotificationsOn = scale.IsNotificationsOn,
                        NotificationDate = scale.NotificationDate,
                        Priority = scale.Priority,
                        Subtasks = scale.Subtasks.Select(SubtaskMapper.ToDto).ToList(),
                        Status = scale.Status,
                        Unit = scale.Unit,
                        CurrentValue = scale.CurrentValue,
                        TargetValue = scale.TargetValue
                    };
                default:
                    var std = (TaskStandart)entity;
                    return new TaskStandartDto
                    {
                        Id = std.Id,
                        UserId = std.UserId,
                        FolderId = std.FolderId,
                        Title = std.Title,
                        Description = std.Description,
                        CategoryId = std.CategoryId,
                        Tags = std.Tags,
                        IsDeadlineOn = std.IsDeadlineOn,
                        Deadline = std.Deadline,
                        IsShownProgressOnPage = std.IsShownProgressOnPage,
                        IsNotificationsOn = std.IsNotificationsOn,
                        NotificationDate = std.NotificationDate,
                        Priority = std.Priority,
                        Subtasks = std.Subtasks.Select(SubtaskMapper.ToDto).ToList(),
                        Status = std.Status
                    };
            }
        }

        public static TaskEntity UpdateEntity(TaskEntity entity, TaskUpdateDto dto)
        {
            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.IsDeadlineOn = dto.IsDeadlineOn;
            entity.Deadline = dto.Deadline;
            entity.IsShownProgressOnPage = dto.IsShownProgressOnPage;
            entity.IsNotificationsOn = dto.IsNotificationsOn;
            entity.NotificationDate = dto.NotificationDate;
            entity.Priority = dto.Priority;
            entity.Subtasks = dto.Subtasks
            .Select(subDto => SubtaskMapper.FromUpdateDto(subDto.Id, subDto))
            .ToList();
            entity.Status = dto.Status;

            switch (entity.Type)
            {
                case "repeatable":
                    var rep = (TaskRepeatable)entity;
                    var repDto = dto as TaskRepeatableUpdateDto;
                    if (repDto != null)
                    {
                        rep.RepeatDays = repDto.RepeatDays;
                        rep.CheckedInDays = repDto.CheckedInDays;
                    }
                    break;
                case "scale":
                    var scale = (TaskScale)entity;
                    var scaleDto = dto as TaskScaleUpdateDto;
                    if (scaleDto != null)
                    {
                        scale.Unit = scaleDto.Unit;
                        scale.CurrentValue = scaleDto.CurrentValue;
                        scale.TargetValue = scaleDto.TargetValue;
                    }
                    break;
            }
            return entity;
        }
    }

}
