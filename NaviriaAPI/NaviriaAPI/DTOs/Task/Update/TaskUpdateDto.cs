using NaviriaAPI.Entities.EmbeddedEntities;
using NaviriaAPI.Helpers;
using NaviriaAPI.DTOs.Task.Subtask.Update;

namespace NaviriaAPI.DTOs.Task.Update
{
    public class TaskUpdateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<Tags> Tags { get; set; } = new();

        public bool IsDeadlineOn { get; set; }
        public DateTime? Deadline { get; set; }

        public bool IsShownProgressOnPage { get; set; }
        public bool IsNotificationsOn { get; set; }
        public DateTime? NotificationDate { get; set; }

        public int Priority { get; set; }
        public string Type { get; set; } = string.Empty;
        public List<SubtaskUpdateDtoBase> Subtasks { get; set; } = new();

        public DateTime? CompletedAt { get; set; }

        public CurrentTaskStatus Status { get; set; }
    }
}
