using Google.Apis.Auth.OAuth2.Requests;
using MongoDB.Bson;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.Entities;
using NaviriaAPI.DTOs.TaskDtos;

namespace NaviriaAPI.Mappings
{
    public static class TaskMapper
    {
        public static TaskEntity ToEntity(TaskCreateDto dto)
        {
            return new TaskEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                UserId = dto.UserId,
                FolderId = dto.FolderId,
                Title = dto.Title,
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                Tags = dto.Tags,
                IsDeadlineOn = dto.IsDeadlineOn,
                Deadline = dto.Deadline,
                IsShownProgressOnPage = dto.IsShownProgressOnPage,
                IsNotificationsOn = dto.IsNotificationsOn,
                NotificationDate = dto.NotificationDate,
                Priority = dto.Priority,
                Subtasks = dto.Subtasks
                    .Select(SubtaskMapper.FromCreateDto)
                    .ToList()
            };
        }

        public static TaskEntity ToEntity(string id, TaskUpdateDto dto)
        {
            return new TaskEntity
            {
                Id = id,
                Title = dto.Title,
                Description = dto.Description,
                Tags = dto.Tags,
                IsDeadlineOn = dto.IsDeadlineOn,
                Deadline = dto.Deadline,
                IsShownProgressOnPage = dto.IsShownProgressOnPage,
                IsNotificationsOn = dto.IsNotificationsOn,
                NotificationDate = dto.NotificationDate,
                Priority = dto.Priority,
                Subtasks = dto.Subtasks
                    .Select(SubtaskMapper.FromUpdateDto)
                    .ToList()


            };
        }

        public static TaskDto ToDto(TaskEntity entity)
        {
            return new TaskDto
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
                Priority = entity.Priority,
                Subtasks = entity.Subtasks.Select(SubtaskMapper.ToDto).ToList()
            };
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
            entity.Subtasks = dto.Subtasks;

            return entity;
        }
    }
}
