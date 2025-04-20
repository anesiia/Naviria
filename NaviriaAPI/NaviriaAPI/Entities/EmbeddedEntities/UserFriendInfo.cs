using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace NaviriaAPI.Entities.EmbeddedEntities
{
    public class UserFriendInfo
    {
        [BsonElement("user_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; } = string.Empty;

        [BsonElement("nickname"), BsonRepresentation(BsonType.String)]
        public string Nickname { get; set; } = string.Empty;

    }
}
