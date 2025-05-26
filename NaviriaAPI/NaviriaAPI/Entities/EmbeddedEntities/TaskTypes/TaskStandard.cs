using MongoDB.Bson.Serialization.Attributes;

namespace NaviriaAPI.Entities.EmbeddedEntities.TaskTypes
{
    [BsonDiscriminator("standard")]
    public class TaskStandard : TaskEntity
    {
        public TaskStandard() { Type = "standard"; }
    }
}
