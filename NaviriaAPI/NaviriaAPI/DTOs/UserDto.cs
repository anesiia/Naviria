using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using NaviriaAPI.Entities.EmbeddedEntities;
using System.ComponentModel.DataAnnotations;

namespace NaviriaAPI.DTOs
{
    public class UserDto
    {
        public string Id { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Nickname { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public DateTime BirthDate { get; set; } = new DateTime(2000, 1, 1);

        public string Description { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public int Points { get; set; } = 0;

        public LevelProgressInfo LevelInfo { get; set; } = new();
        public string[] Friends { get; set; } = [];

        public List<UserAchievementInfo> Achievements { get; set; } = new();

        public string FutureMessage { get; set; } = string.Empty;

        public string Photo { get; set; } = string.Empty;

        public DateTime LastSeen { get; set; } = DateTime.Now;
        public bool IsOnline { get; set; } = false;
        public bool IsProUser { get; set; } = false;
    }
}
