using MongoDB.Bson.Serialization.Attributes;

namespace NaviriaAPI.Entities.EmbeddedEntities
{
    public class LevelProgressInfo
    {
        [BsonElement("level")]
        public int Level { get; set; }

        [BsonElement("total_xp")]
        public int TotalXp { get; set; }

        [BsonElement("xp_for_next_level")]
        public int XpForNextLevel { get; set; }

        [BsonElement("progress")]
        public double Progress { get; set; } // from 0.0 to 1.0
    }

}
