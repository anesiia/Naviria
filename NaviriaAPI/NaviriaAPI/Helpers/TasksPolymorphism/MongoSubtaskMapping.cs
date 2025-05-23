using MongoDB.Bson.Serialization;
using NaviriaAPI.Entities.EmbeddedEntities.Subtasks;

namespace NaviriaAPI.Helpers.TasksPolymorphism
{
    public static class MongoSubtaskMapping
    {
        private static bool _registered = false;

        public static void RegisterSubtaskMappings()
        {
            if (_registered) return;

            if (!BsonClassMap.IsClassMapRegistered(typeof(SubtaskBase)))
            {
                BsonClassMap.RegisterClassMap<SubtaskBase>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIsRootClass(true);
                    cm.SetDiscriminator("subtask_type");
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(SubtaskStandard)))
            {
                BsonClassMap.RegisterClassMap<SubtaskStandard>(cm =>
                {
                    cm.AutoMap();
                    cm.SetDiscriminator("standard");
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(SubtaskRepeatable)))
            {
                BsonClassMap.RegisterClassMap<SubtaskRepeatable>(cm =>
                {
                    cm.AutoMap();
                    cm.SetDiscriminator("repeatable");
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(ScaleSubtask)))
            {
                BsonClassMap.RegisterClassMap<ScaleSubtask>(cm =>
                {
                    cm.AutoMap();
                    cm.SetDiscriminator("scale");
                });
            }

            _registered = true;
        }
    }
}