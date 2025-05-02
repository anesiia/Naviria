using Google.Apis.Auth.OAuth2.Requests;
using MongoDB.Bson;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.DTOs;
using NaviriaAPI.Entities;

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
                Name = dto.Name,
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
            };
        }

        public static TaskEntity ToEntity(string id, TaskUpdateDto dto)
        {
            return new TaskEntity
            {
                Id = id,
                Name = dto.Name,
                Description = dto.Description,
                Tags = dto.Tags,
                IsDeadlineOn = dto.IsDeadlineOn,
                Deadline = dto.Deadline,
                IsShownProgressOnPage = dto.IsShownProgressOnPage,
                IsNotificationsOn = dto.IsNotificationsOn,
                NotificationDate = dto.NotificationDate,
                Priority = dto.Priority,
                Subtasks = dto.Subtasks
            };
        }

        public static TaskDto ToDto(TaskEntity entity)
        {
            return new TaskDto
            {
                Id = entity.Id,
                UserId = entity.UserId,
                FolderId = entity.FolderId,
                Name = entity.Name,
                Description = entity.Description,
                CategoryId = entity.CategoryId,
                Tags = entity.Tags,
                IsDeadlineOn = entity.IsDeadlineOn,
                Deadline = entity.Deadline,
                IsShownProgressOnPage = entity.IsShownProgressOnPage,
                IsNotificationsOn = entity.IsNotificationsOn,
                NotificationDate = entity.NotificationDate,
                Priority = entity.Priority,
                Subtasks = entity.Subtasks
            };
        }
    }
}
