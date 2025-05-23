using MongoDB.Bson.Serialization.Attributes;

namespace NaviriaAPI.Entities.EmbeddedEntities.TaskTypes
{
    [BsonDiscriminator("repeatable")]
    public class TaskRepeatable : TaskEntity
    {
        [BsonElement("repeat_days")]
        public List<DayOfWeek> RepeatDays { get; set; } = new();

        [BsonElement("chechked_in_days")]
        public List<DateTime> CheckedInDays { get; set; } = new();

        public TaskRepeatable()
        {
            Type = "repeatable";
        }
    }
}
