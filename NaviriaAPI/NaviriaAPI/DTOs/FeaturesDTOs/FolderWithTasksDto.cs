using NaviriaAPI.DTOs.Task.View;

namespace NaviriaAPI.DTOs.FeaturesDTOs
{
    public class FolderWithTasksDto
    {
        public string FolderId { get; set; } = string.Empty;
        public string FolderName { get; set; } = string.Empty;
        public List<TaskDto> Tasks { get; set; } = new();
    }
}
