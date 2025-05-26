using MongoDB.Bson.Serialization;
using NaviriaAPI.Entities;
using NaviriaAPI.Entities.EmbeddedEntities.TaskTypes;

namespace NaviriaAPI.Helpers.TasksPolymorphism
{
    public static class MongoTaskMapping
    {
        private static bool _registered = false;

        public static void RegisterTaskMappings()
        {
            if (_registered) return;

            if (!BsonClassMap.IsClassMapRegistered(typeof(TaskEntity)))
            {
                BsonClassMap.RegisterClassMap<TaskEntity>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIsRootClass(true);
                    cm.SetDiscriminator("task_type");
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(TaskStandard)))
            {
                BsonClassMap.RegisterClassMap<TaskStandard>(cm =>
                {
                    cm.AutoMap();
                    cm.SetDiscriminator("standard");
                });
            }
            if (!BsonClassMap.IsClassMapRegistered(typeof(TaskScale)))
            {
                BsonClassMap.RegisterClassMap<TaskScale>(cm =>
                {
                    cm.AutoMap();
                    cm.SetDiscriminator("scale");
                });
            }
            if (!BsonClassMap.IsClassMapRegistered(typeof(TaskRepeatable)))
            {
                BsonClassMap.RegisterClassMap<TaskRepeatable>(cm =>
                {
                    cm.AutoMap();
                    cm.SetDiscriminator("repeatable");
                });
            }
            if (!BsonClassMap.IsClassMapRegistered(typeof(TaskWithSubtasks)))
            {
                BsonClassMap.RegisterClassMap<TaskWithSubtasks>(cm =>
                {
                    cm.AutoMap();
                    cm.SetDiscriminator("with_subtasks");
                });
            }

            _registered = true;
        }
    }
}
