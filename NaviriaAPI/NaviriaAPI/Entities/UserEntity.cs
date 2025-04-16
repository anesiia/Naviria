using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using NaviriaAPI.Entities.EmbeddedEntities;
using System.ComponentModel.DataAnnotations;

namespace NaviriaAPI.Entities
{
    public class UserEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("full_name"), BsonRepresentation(BsonType.String)]
        public string FullName { get; set; } = string.Empty;

        [BsonElement("nickname"), BsonRepresentation(BsonType.String)]
        public string Nickname { get; set; } = string.Empty;

        [BsonElement("gender"), BsonRepresentation(BsonType.String)]
        public string Gender { get; set; } = string.Empty;

        [BsonElement("birth_date"), BsonRepresentation(BsonType.DateTime)]
        public DateTime BirthDate { get; set; } = new DateTime(2000, 1, 1); // local or universal time

        [BsonElement("description"), BsonRepresentation(BsonType.String)]
        public string Description { get; set; } = string.Empty;

        [BsonElement("email"), BsonRepresentation(BsonType.String)]
        public string Email { get; set; } = string.Empty;

        [BsonElement("password"), BsonRepresentation(BsonType.String)]
        public string Password { get; set; } = string.Empty;

        [BsonElement("points"), BsonRepresentation(BsonType.Int32)]
        public int Points { get; set; } = 0;

        [BsonElement("level_info")]
        public LevelProgressInfo LevelInfo { get; set; } = new();

        [BsonElement("friends")]
        public string[] Friends { get; set; } = [];

        [BsonElement("achievements")]
        public List<UserAchievementInfo> Achievements { get; set; } = new();


        [BsonElement("future_message"), BsonRepresentation(BsonType.String)]
        public string FutureMessage { get; set; } = string.Empty;

        [BsonElement("photo"), BsonRepresentation(BsonType.String)]
        public string Photo { get; set; } = string.Empty; //link to photo in Google Disk

        [BsonElement("LastSeen"), BsonRepresentation(BsonType.DateTime)]
        public DateTime LastSeen { get; set; } = DateTime.Now; // local or universal time

        [BsonElement("is_online"), BsonRepresentation(BsonType.Boolean)]
        public bool IsOnline { get; set; } = false;

        [BsonElement("is_pro_user"), BsonRepresentation(BsonType.Boolean)]
        public bool IsProUser { get; set; } = false;
    }
}
