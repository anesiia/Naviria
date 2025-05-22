using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using NaviriaAPI.Entities.EmbeddedEntities;
using NaviriaAPI.Helpers;

namespace NaviriaAPI.DTOs.CreateDTOs
{
    public class TaskCreateDto
    {
        public string UserId { get; set; } = string.Empty;
        public string FolderId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CategoryId { get; set; } = string.Empty;
        public List<Tags> Tags { get; set; } = new();

        public bool IsDeadlineOn { get; set; }
        public DateTime? Deadline { get; set; }

        public bool IsShownProgressOnPage { get; set; }
        public bool IsNotificationsOn { get; set; }
        public DateTime? NotificationDate { get; set; }

        public int Priority { get; set; }
        public List<SubtaskCreateDtoBase> Subtasks { get; set; } = new();
        public CurrentTaskStatus Status { get; set; }
    }
}
