namespace NaviriaAPI.DTOs
{
    public class NotificationDto
    {
        public required string Id { get; set; }
        public required string UserId { get; set; }

        public required string Text { get; set; }

        public DateTime RecievedAt { get; set; }
        public bool IsNew { get; set; }
    }
}
