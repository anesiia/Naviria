﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using NaviriaAPI.Entities.EmbeddedEntities;
using NaviriaAPI.Entities.EmbeddedEntities.Subtasks;
using NaviriaAPI.Helpers;

namespace NaviriaAPI.Entities
{
    public class TaskEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("user_id"), BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; } = string.Empty;

        [BsonElement("folder_id"), BsonRepresentation(BsonType.ObjectId)]
        public string FolderId { get; set; } = string.Empty;

        [BsonElement("Title"), BsonRepresentation(BsonType.String)]
        public string Title { get; set; } = string.Empty;

        [BsonElement("description"), BsonRepresentation(BsonType.String)]
        public string Description { get; set; } = string.Empty;

        [BsonElement("category_id"), BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId { get; set; } = string.Empty;

        [BsonElement("tags")]
        public List<Tags> Tags { get; set; } = new();

        [BsonElement("is_deadline_on"), BsonRepresentation(BsonType.Boolean)]
        public bool IsDeadlineOn { get; set; }

        [BsonElement("deadline"), BsonRepresentation(BsonType.DateTime)]
        public DateTime? Deadline { get; set; }

        [BsonElement("is_progress_shown"), BsonRepresentation(BsonType.Boolean)]
        public bool IsShownProgressOnPage { get; set; }

        [BsonElement("is_notification_on"), BsonRepresentation(BsonType.Boolean)]
        public bool IsNotificationsOn { get; set; }

        [BsonElement("notification_date"), BsonRepresentation(BsonType.DateTime)]
        public DateTime? NotificationDate { get; set; }

        [BsonElement("notification_sent")]
        public bool NotificationSent { get; set; }

        [BsonElement("priority"), BsonRepresentation(BsonType.Int32)]
        public int Priority { get; set; } // from 1 to 10

        [BsonElement("task_type")]
        public string Type { get; set; } = string.Empty;

        [BsonElement("subtasks")]
        public List<SubtaskBase> Subtasks { get; set; } = new();

        [BsonElement("created_at"), BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; set; }

        [BsonElement("completed_at"), BsonRepresentation(BsonType.DateTime)]
        public DateTime? CompletedAt { get; set; }

        [BsonElement("status"), BsonRepresentation(BsonType.String)]
        public CurrentTaskStatus Status { get; set; }

    }

}
