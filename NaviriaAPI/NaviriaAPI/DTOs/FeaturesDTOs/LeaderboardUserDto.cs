namespace NaviriaAPI.DTOs.FeaturesDTOs
{
    public class LeaderboardUserDto
    {
        public string UserId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public int Level { get; set; }
        public int Points { get; set; }
        public double CompletionRate { get; set; } // 0..1
        public int AchievementsCount { get; set; }
        public string Photo { get; set; } = string.Empty;
    }
}
