using System.ComponentModel.DataAnnotations;

namespace NaviriaAPI.DTOs.CreateDTOs
{
    public class NotificationCreateDto
    {
        public required string UserId { get; set; }

        [MaxLength(150)]
        public required string Text { get; set; }

        public DateTime RecievedAt { get; set; }

        public bool IsNew {  get; set; }
    }
}
