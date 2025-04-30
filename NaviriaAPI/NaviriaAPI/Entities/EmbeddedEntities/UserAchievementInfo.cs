using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace NaviriaAPI.Entities.EmbeddedEntities
{
    public class UserAchievementInfo
    {
        [BsonElement("achievement_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string AchievementId { get; set; } = string.Empty;

        [BsonElement("received_at")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime? ReceivedAt { get; set; }

        [BsonElement("is_points_received")]
        public bool IsPointsReceived { get; set; }
    }
}
