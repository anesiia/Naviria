namespace NaviriaAPI.DTOs.FeaturesDTOs
{
    public class CategoryPieChartDto
    {
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public double Value { get; set; } // 1..100
    }
}
