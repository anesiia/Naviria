namespace NaviriaAPI.Entities.EmbeddedEntities
{
    public class ScaleSubtask : SubtaskBase
    {
        public string Unit { get; set; } = string.Empty;
        public double CurrentValue { get; set; }
        public double TargetValue { get; set; }
    }
}
