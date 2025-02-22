namespace Core.View.DayType
{
    public class GetDayTypeView
    {
        public long DayTypeID { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public TimeSpan ShiftTime { get; set; }
        public TimeSpan BreakTime { get; set; }
        public TimeSpan ShiftTimeWithoutBreak { get; set; }
        public List<AdditionalHoursView> AdditionalHours { get; set; }
    }
}
