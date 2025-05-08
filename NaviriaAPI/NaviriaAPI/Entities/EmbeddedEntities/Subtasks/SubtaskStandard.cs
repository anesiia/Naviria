using MongoDB.Bson.Serialization.Attributes;

namespace NaviriaAPI.Entities.EmbeddedEntities.Subtasks
{
    [BsonDiscriminator("standard")]
    public class SubtaskStandard : SubtaskBase
    {
        [BsonElement("is_completed")]
        public bool IsCompleted { get; set; }

        public SubtaskStandard()
        {
            Type = "standard";
        }
    }
}
