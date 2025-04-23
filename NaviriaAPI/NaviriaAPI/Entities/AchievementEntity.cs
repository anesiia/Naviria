using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace NaviriaAPI.Entities
{
    public class AchievementEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("points")]
        public int Points { get; set; } = 0;

        [BsonElement("is_rare")]
        public bool IsRare {  get; set; }
    }
}
