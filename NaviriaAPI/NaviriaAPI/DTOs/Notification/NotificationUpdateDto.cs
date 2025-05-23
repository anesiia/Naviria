using System.ComponentModel.DataAnnotations;

namespace NaviriaAPI.DTOs.Notification
{
    public class NotificationUpdateDto
    {
        public required string UserId { get; set; }
        public bool IsNew { get; set; }
    }
}
