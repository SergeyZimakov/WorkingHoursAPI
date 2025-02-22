using Core.View.Statistics;

namespace Core.View.Shift
{
    public class GetShiftView
    {
        public long ShiftID { get; set; }
        public DateTime Date { get; set; }
        public StartStopView StartStop { get; set; } = new StartStopView();
        public ShiftDayTypeView? DayType { get; set; } = null;
        public StatisticsView? Statistics { get; set; } = null;
        public List<StartStopView> Pauses { get; set; } = new List<StartStopView>();
    }
}
