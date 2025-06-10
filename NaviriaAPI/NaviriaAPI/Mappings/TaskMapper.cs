using MongoDB.Bson;
using NaviriaAPI.DTOs.Task.Subtask.Create;
using NaviriaAPI.DTOs.Task.Subtask.Update;
using NaviriaAPI.DTOs.Task.Subtask;
using NaviriaAPI.DTOs.Task.Subtask.View;
using NaviriaAPI.DTOs.TaskDtos;
using NaviriaAPI.Entities.EmbeddedEntities.TaskTypes;
using NaviriaAPI.Entities;
using NaviriaAPI.Helpers;
using NaviriaAPI.DTOs.Task.Create;
using NaviriaAPI.DTOs.Task.Update;
using NaviriaAPI.DTOs.Task.View;

namespace NaviriaAPI.Mappings
{
    public static class TaskMapper
    {
        public static TaskEntity ToEntity(TaskCreateDto dto)
        {
            switch (dto.Type)
            {
                case "with_subtasks":
                    var withSub = (TaskWithSubtasksCreateDto)dto;
                    return new TaskWithSubtasks
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        UserId = withSub.UserId,
                        FolderId = withSub.FolderId,
                        Title = withSub.Title,
                        Description = withSub.Description,
                        CategoryId = withSub.CategoryId,
                        Tags = withSub.Tags,
                        IsDeadlineOn = withSub.IsDeadlineOn,
                        Deadline = withSub.Deadline,
                        IsShownProgressOnPage = withSub.IsShownProgressOnPage,
                        IsNotificationsOn = withSub.IsNotificationsOn,
                        NotificationDate = withSub.NotificationDate,
                        NotificationSent = false,
                        Priority = withSub.Priority,
                        Subtasks = withSub.Subtasks.Select(x => SubtaskMapper.FromCreateDto(x)).ToList(),
                        CreatedAt = DateTime.UtcNow,
                        Status = CurrentTaskStatus.InProgress
                    };
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
                        NotificationSent = false,
                        Priority = rep.Priority,
                        Subtasks = rep.Subtasks.Select(x => SubtaskMapper.FromCreateDto(x)).ToList(),
                        CreatedAt = DateTime.UtcNow,
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
                        NotificationSent = false,
                        Priority = scale.Priority,
                        Subtasks = scale.Subtasks.Select(x => SubtaskMapper.FromCreateDto(x)).ToList(),
                        CreatedAt = DateTime.UtcNow,
                        Status = CurrentTaskStatus.InProgress,
                        Unit = scale.Unit,
                        CurrentValue = scale.CurrentValue,
                        TargetValue = scale.TargetValue
                    };
                default:
                    var std = (TaskStandardCreateDto)dto;
                    return new TaskStandard
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
                        NotificationSent = false,
                        Priority = std.Priority,
                        Subtasks = std.Subtasks.Select(x => SubtaskMapper.FromCreateDto(x)).ToList(),
                        CreatedAt = DateTime.UtcNow,
                        Status = CurrentTaskStatus.InProgress
                    };
            }
        }

        public static TaskDto ToDto(TaskEntity entity)
        {
            switch (entity.Type)
            {
                case "with_subtasks":
                    if (entity is TaskWithSubtasks withSub)
                    {
                        return new TaskWithSubtasksDto
                        {
                            Id = withSub.Id,
                            UserId = withSub.UserId,
                            FolderId = withSub.FolderId,
                            Title = withSub.Title,
                            Description = withSub.Description,
                            CategoryId = withSub.CategoryId,
                            Tags = withSub.Tags,
                            IsDeadlineOn = withSub.IsDeadlineOn,
                            Deadline = withSub.Deadline,
                            IsShownProgressOnPage = withSub.IsShownProgressOnPage,
                            IsNotificationsOn = withSub.IsNotificationsOn,
                            NotificationDate = withSub.NotificationDate,
                            NotificationSent = withSub.NotificationSent,
                            Priority = withSub.Priority,
                            Subtasks = withSub.Subtasks.Select(SubtaskMapper.ToDto).ToList(),
                            CreatedAt = withSub.CreatedAt,
                            CompletedAt = withSub.CompletedAt,
                            Status = withSub.Status
                        };
                    }
                    break;
                case "repeatable":
                    if (entity is TaskRepeatable rep)
                    {
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
                            NotificationSent = rep.NotificationSent,
                            Priority = rep.Priority,
                            //Subtasks = rep.Subtasks.Select(SubtaskMapper.ToDto).ToList(),
                            CreatedAt = rep.CreatedAt,
                            CompletedAt = rep.CompletedAt,
                            Status = rep.Status,
                            RepeatDays = rep.RepeatDays,
                            CheckedInDays = rep.CheckedInDays
                        };
                    }
                    break;
                case "scale":
                    if (entity is TaskScale scale)
                    {
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
                            NotificationSent = scale.NotificationSent,
                            Priority = scale.Priority,
                            //Subtasks = scale.Subtasks.Select(SubtaskMapper.ToDto).ToList(),
                            CreatedAt = scale.CreatedAt,
                            CompletedAt = scale.CompletedAt,
                            Status = scale.Status,
                            Unit = scale.Unit,
                            CurrentValue = scale.CurrentValue,
                            TargetValue = scale.TargetValue
                        };
                    }
                    break;
                default:
                    if (entity is TaskStandard std)
                    {
                        return new TaskStandardDto
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
                            NotificationSent = std.NotificationSent,
                            Priority = std.Priority,
                            //Subtasks = std.Subtasks.Select(SubtaskMapper.ToDto).ToList(),
                            CreatedAt = std.CreatedAt,
                            CompletedAt = std.CompletedAt,
                            Status = std.Status
                        };
                    }

                    return new TaskStandardDto
                    {
                        Id = entity.Id,
                        UserId = entity.UserId,
                        FolderId = entity.FolderId,
                        Title = entity.Title,
                        Description = entity.Description,
                        CategoryId = entity.CategoryId,
                        Tags = entity.Tags,
                        IsDeadlineOn = entity.IsDeadlineOn,
                        Deadline = entity.Deadline,
                        IsShownProgressOnPage = entity.IsShownProgressOnPage,
                        IsNotificationsOn = entity.IsNotificationsOn,
                        NotificationDate = entity.NotificationDate,
                        NotificationSent = entity.NotificationSent,
                        Priority = entity.Priority,
                        //Subtasks = entity.Subtasks.Select(SubtaskMapper.ToDto).ToList(),
                        CreatedAt = entity.CreatedAt,
                        CompletedAt = entity.CompletedAt,
                        Status = entity.Status
                    };
            }
            throw new InvalidOperationException($"Unknown or invalid task type: {entity.Type}");
        }


        public static TaskEntity UpdateEntity(TaskEntity entity, TaskUpdateDto dto)
        {
            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.Tags = dto.Tags;
            entity.IsDeadlineOn = dto.IsDeadlineOn;
            entity.Deadline = dto.Deadline;
            entity.IsShownProgressOnPage = dto.IsShownProgressOnPage;
            entity.IsNotificationsOn = dto.IsNotificationsOn;
            entity.NotificationDate = dto.NotificationDate;
            entity.Priority = dto.Priority;
            entity.Subtasks = dto.Subtasks
            .Select(subDto => SubtaskMapper.FromUpdateDto(subDto.Id, subDto))
            .ToList();
            entity.CompletedAt = dto.CompletedAt;
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
