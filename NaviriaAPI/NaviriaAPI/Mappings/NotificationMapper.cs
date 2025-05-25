using NaviriaAPI.Entities;
using NaviriaAPI.DTOs.Notification;

namespace NaviriaAPI.Mappings
{
    public class NotificationMapper
    {
        public static NotificationDto ToDto(NotificationEntity entity) =>
            new NotificationDto
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Text = entity.Text,
                RecievedAt = entity.RecievedAt,
                IsNew = entity.IsNew
            };

        public static NotificationEntity ToEntity(NotificationDto dto) =>
            new NotificationEntity
            {
                Id = dto.Id,
                UserId = dto.UserId,
                Text = dto.Text,
                RecievedAt = dto.RecievedAt,
                IsNew = dto.IsNew
            };

        public static NotificationEntity ToEntity(NotificationCreateDto dto) =>
            new NotificationEntity
            {
                UserId = dto.UserId,
                Text = dto.Text,
                RecievedAt = dto.RecievedAt,
                IsNew = true
            };

        public static NotificationEntity ToEntity(string id, NotificationUpdateDto dto) =>
            new NotificationEntity
            {
                UserId = dto.UserId,
                IsNew = dto.IsNew
            };
    }
}
