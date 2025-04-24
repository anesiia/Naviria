namespace NaviriaAPI.DTOs
{
    public class AchievementDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Points { get; set; } = 0;
        public bool IsRare {  get; set; }

    }
}
