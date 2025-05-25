using MongoDB.Bson.Serialization.Attributes;

namespace NaviriaAPI.Entities.EmbeddedEntities.TaskTypes
{
    [BsonDiscriminator("standart")]
    public class TaskStandart : TaskEntity
    {
        public TaskStandart() { Type = "standart"; }
    }
}
