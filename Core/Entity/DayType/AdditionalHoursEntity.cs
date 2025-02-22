namespace Core.Entity.DayType
{
    public class AdditionalHoursEntity
    {
        public long DayTypeID { get; set; }
        public TimeSpan Hours { get; set; }
        public int Percentage { get; set; }
        public int Order { get; set; }
        public DayTypeEntity DayType { get; set; }
    }
}
