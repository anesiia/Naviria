using MongoDB.Bson.Serialization.Attributes;

namespace NaviriaAPI.Entities.EmbeddedEntities.Subtasks
{
    [BsonDiscriminator("scale")]
    public class ScaleSubtask : SubtaskBase
    {
        [BsonElement("unit")]
        public string Unit { get; set; } = string.Empty;

        [BsonElement("current_value")]
        public double CurrentValue { get; set; }

        [BsonElement("target_value")]
        public double TargetValue { get; set; }

        public ScaleSubtask()
        {
            Type = "scale";
        }
    }
}
