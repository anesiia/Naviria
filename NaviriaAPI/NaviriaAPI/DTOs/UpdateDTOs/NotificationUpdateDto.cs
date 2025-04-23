using System.ComponentModel.DataAnnotations;

namespace NaviriaAPI.DTOs.UpdateDTOs
{
    public class NotificationUpdateDto
    {
        public required string UserId { get; set; }
        public bool IsNew { get; set; }
    }
}
