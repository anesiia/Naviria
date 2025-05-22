namespace NaviriaAPI.DTOs.CreateDTOs
{
    public class SubtaskStandardCreateDto : SubtaskCreateDtoBase
    {
        public bool IsCompleted { get; set; }
        public SubtaskStandardCreateDto()
        {
            Type = "standard";
        }
    }
}