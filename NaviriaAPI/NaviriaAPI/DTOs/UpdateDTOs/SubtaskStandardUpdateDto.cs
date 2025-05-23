namespace NaviriaAPI.DTOs.UpdateDTOs
{
    public class SubtaskStandardUpdateDto : SubtaskUpdateDtoBase
    {
        public bool IsCompleted { get; set; }
        public SubtaskStandardUpdateDto()
        {
            Type = "standard";
        }
    }
}