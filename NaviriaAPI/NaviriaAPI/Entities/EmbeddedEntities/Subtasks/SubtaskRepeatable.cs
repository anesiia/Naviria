using MongoDB.Bson.Serialization.Attributes;

namespace NaviriaAPI.Entities.EmbeddedEntities.Subtasks
{
    [BsonDiscriminator("repeatable")]
    public class SubtaskRepeatable : SubtaskBase
    {
        [BsonElement("repeat_days")]
        public List<DayOfWeek> RepeatDays { get; set; } = new();

        public SubtaskRepeatable()
        {
            Type = "repeatable";
        }
    }
}
