using MongoDB.Bson.Serialization.Attributes;

namespace NaviriaAPI.Entities.EmbeddedEntities
{
    [BsonKnownTypes(typeof(SubtaskStandard), typeof(SubtaskRepeatable), typeof(ScaleSubtask))]
    [BsonDiscriminator("subtask_type")]
    public abstract class SubtaskBase
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

}
