namespace Core.View.Statistics
{
    public class StatisticsView
    {
        public TimeSpan RequiredHours { get; set; }
        public TimeSpan ActualHours { get; set; }
        public TimeSpan TotalHours { get; set; }
        public TimeSpan Break { get; set; }
        public TimeSpan Balance { get; set; }
        public List<AdditionalHoursStatisticsView> AdditionalHours { get; set; } = new List<AdditionalHoursStatisticsView>();
    }
}
