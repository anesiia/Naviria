using System.ComponentModel.DataAnnotations;

namespace NaviriaAPI.DTOs.Notification
{
    public class NotificationCreateDto
    {
        public required string UserId { get; set; }

        [MaxLength(150)]
        public required string Text { get; set; }

        public DateTime RecievedAt { get; set; }
    }
}
