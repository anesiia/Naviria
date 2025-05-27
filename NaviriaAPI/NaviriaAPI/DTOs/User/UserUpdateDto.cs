using NaviriaAPI.Entities.EmbeddedEntities;
using System.ComponentModel.DataAnnotations;

namespace NaviriaAPI.DTOs.User
{
    public class UserUpdateDto
    {
        [Required]
        [MaxLength(50)]
        [MinLength(3)]
        [RegularExpression("^[a-zA-Zа-яА-ЯёЁіІїЇєЄ\\s'-]{1,50}$")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        [MinLength(3)]
        [RegularExpression("^[a-zA-Z0-9]+$")]
        public string Nickname { get; set; } = string.Empty;

        [MaxLength(150)]
        [RegularExpression("^[a-zA-Zа-яА-ЯёЁіІїЇєЄ0-9.,!\\s]{0,150}$")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$")]
        public string Email { get; set; } = string.Empty;
        
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).+$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one digit.")]
        public string? Password { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Points { get; set; } = 0;

        [Required]
        public LevelProgressInfo LevelInfo { get; set; } = new();

        public List<UserFriendInfo> Friends { get; set; } = new();

        public List<UserAchievementInfo> Achievements { get; set; } = new();

        [MaxLength(150)]
        [RegularExpression("^[a-zA-Zа-яА-ЯёЁіІїЇєЄ0-9.,!\\s]{0,150}$")]
        public string FutureMessage { get; set; } = string.Empty;

        [Url]
        public string? Photo { get; set; }
        public DateTime? LastSeen { get; set; }
        public bool IsOnline { get; set; } = false;
        public bool IsProUser { get; set; } = false;
    }

}
