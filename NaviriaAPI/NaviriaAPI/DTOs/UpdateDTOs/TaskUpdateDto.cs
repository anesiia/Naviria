using NaviriaAPI.Entities.EmbeddedEntities;

namespace NaviriaAPI.DTOs.UpdateDTOs
{
    public class TaskUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
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
