namespace Core.Helper
{
    public static class DateTimeHelper
    {
        public static IEnumerable<DateTime> GetDaysByMonthAndYear(int year, int month)
        {
            int numDays = DateTime.DaysInMonth(year, month);
            return Enumerable
                .Range(1, numDays)
                .Select(day => new DateTime(year, month, day));
        }
    }
}
