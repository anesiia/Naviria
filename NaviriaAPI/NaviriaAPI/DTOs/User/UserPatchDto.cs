using NaviriaAPI.Entities.EmbeddedEntities;
using System.ComponentModel.DataAnnotations;

namespace NaviriaAPI.DTOs.User
{
    public class UserPatchDto
    {
        [MaxLength(50)]
        [MinLength(3)]
        public string? FullName { get; set; }

        [MaxLength(20)]
        [MinLength(3)]
        public string? Nickname { get; set; }

        [MaxLength(150)]
        public string? Description { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [MinLength(8)]
        public string? Password { get; set; }

        public int? Points { get; set; }
        public string? FutureMessage { get; set; }
        [Url]
        public string? Photo { get; set; }
        public bool? IsOnline { get; set; }
        public bool? IsProUser { get; set; }

        public LevelProgressInfo? LevelInfo { get; set; } = new();

        public List<UserFriendInfo>? Friends { get; set; } = new();

        public List<UserAchievementInfo>? Achievements { get; set; } = new();

    }

}
