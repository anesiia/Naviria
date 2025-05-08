using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace NaviriaAPI.Entities.EmbeddedEntities.Subtasks
{
    public abstract class SubtaskBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("title"), BsonRepresentation(BsonType.String)]
        public string Title { get; set; } = string.Empty;

        [BsonElement("description"), BsonRepresentation(BsonType.String)]
        public string Description { get; set; } = string.Empty;

        [BsonElement("subtask_type"), BsonRepresentation(BsonType.String)]
        public string Type { get; set; } = string.Empty;
    }

}


