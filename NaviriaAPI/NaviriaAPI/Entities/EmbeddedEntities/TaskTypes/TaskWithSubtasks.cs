using MongoDB.Bson.Serialization.Attributes;

namespace NaviriaAPI.Entities.EmbeddedEntities.TaskTypes
{
    [BsonDiscriminator("with_subtasks")]
    public class TaskWithSubtasks : TaskEntity
    {
        public TaskWithSubtasks()
        {
            Type = "with_subtasks";
        }
    }
}
