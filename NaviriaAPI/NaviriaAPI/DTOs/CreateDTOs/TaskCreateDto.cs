using NaviriaAPI.Entities.EmbeddedEntities;

namespace NaviriaAPI.DTOs.CreateDTOs
{
    public class TaskCreateDto
    {
        public string UserId { get; set; } = string.Empty;
        public string FolderId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CategoryId { get; set; } = string.Empty;
        public List<Tags> Tags { get; set; } = new();

        public bool IsDeadlineOn { get; set; }
        public DateTime? Deadline { get; set; }

        public bool IsShownProgressOnPage { get; set; }
        public bool IsNotificationsOn { get; set; }
        public DateTime? NotificationDate { get; set; }

        public int Priority { get; set; }
        public List<SubtaskBase> Subtasks { get; set; } = new();
    }
}
