using System.ComponentModel.DataAnnotations;

namespace NaviriaAPI.DTOs.FeaturesDTOs
{
    public class AssistantChatMessageDto
    {
        public required string UserId { get; set; }

        [Required]
        [MaxLength(3000, ErrorMessage = "Message is too long.")]
        [MinLength(1, ErrorMessage = "Message is too short.")]
        [RegularExpression(@"^[^<>]*$", ErrorMessage = "Message contains potentially unsafe characters.")]
        public string Message { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public bool IsTaskRequest { get; set; } = false;
    }
}
