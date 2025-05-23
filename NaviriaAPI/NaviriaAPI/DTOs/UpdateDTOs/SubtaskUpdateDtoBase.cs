using NaviriaAPI.Helpers;

namespace NaviriaAPI.DTOs.UpdateDTOs
{
    public class SubtaskUpdateDtoBase
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }
}