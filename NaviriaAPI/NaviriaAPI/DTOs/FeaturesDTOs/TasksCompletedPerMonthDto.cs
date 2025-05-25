namespace NaviriaAPI.DTOs.FeaturesDTOs
{
    public class TasksCompletedPerMonthDto
    {
        public int Year { get; set; }
        public int Month { get; set; } // 1 = January, 12 = December
        public int CompletedCount { get; set; }
    }
}
