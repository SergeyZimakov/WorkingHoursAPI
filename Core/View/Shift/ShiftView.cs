namespace Core.View.Shift
{
    public class ShiftView
    {
        public DateTime Date { get; set; }
        public StartStopView StartStop { get; set; } = new StartStopView();
        public long? DayTypeID { get; set; } = null;
        public List<StartStopView> Pauses { get; set; } = new List<StartStopView>();
    }
}
