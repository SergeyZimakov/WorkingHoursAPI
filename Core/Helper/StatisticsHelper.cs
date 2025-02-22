using Core.Entity.DayType;
using Core.Entity.Shift;
using Core.View.Statistics;

namespace Core.Helper
{
    public static class StatisticsHelper
    {
        public static StatisticsView? GetStatistics(DayTypeEntity dayType, ShiftEntity shift)
        {
            if (!shift.Stop.HasValue)
                return null;

            var totalHours = CountTotalHours(shift);
            var actualHours = totalHours < dayType.ShiftTimeWithoutBreak ? totalHours : (totalHours - dayType.BreakTime);
            var breakHours = totalHours < dayType.ShiftTimeWithoutBreak ? TimeSpan.Zero : dayType.BreakTime;
            var requiredActualHours = dayType.ShiftTime - dayType.BreakTime;
            var balance = actualHours - requiredActualHours;

            var additionalHours = new List<AdditionalHoursStatisticsView>();
            if (balance > TimeSpan.Zero && dayType.AdditionalHours.Any())
            {
                var rest = balance;
                foreach (var item in dayType.AdditionalHours)
                {
                    if (rest <= TimeSpan.Zero)
                        break;

                    additionalHours.Add(new AdditionalHoursStatisticsView
                    {
                        Hours = rest <= item.Hours ? rest : item.Hours,
                        Percentage = item.Percentage
                    });

                    rest -= item.Hours;
                }
            }

            var statistics = new StatisticsView
            {
                ActualHours = actualHours,
                Break = breakHours,
                RequiredHours = requiredActualHours,
                TotalHours = totalHours,
                Balance = balance,
                AdditionalHours = additionalHours,
            };
            return statistics;
        }

        public static CurrentBalanceView GetCurrentBalance(List<DayTypeEntity> dayTypes, List<ShiftEntity> shifts)
        {
            var statisticsList = new List<StatisticsView>();
            foreach (var shift in shifts)
            {
                if (!shift.DayTypeID.HasValue) continue;
               
                var dayType = dayTypes.Find(dt => dt.DayTypeID == shift.DayTypeID.Value);
                if (dayType == null) continue;

                var statistics = GetStatistics(dayType, shift);
                if (statistics == null) continue;

                statisticsList.Add(statistics);
            }

            return new CurrentBalanceView
            {
                Required = statisticsList.Aggregate(TimeSpan.Zero, (current, item) => current + item.RequiredHours),
                Actual = statisticsList.Aggregate(TimeSpan.Zero, (current, item) => current + item.ActualHours)
            };
        }

        public static TimeSpan CountTotalHours(ShiftEntity shift)
        {
            if (!shift.Stop.HasValue)
                return TimeSpan.Zero;

            var fromStartToEnd = (DateTime)shift.Stop - shift.Start;
            var pauses = shift.ShiftPauses
                .Where(p => p.Stop.HasValue)
                .Aggregate(TimeSpan.Zero, (sum, obj) => sum + (TimeSpan)(obj.Stop - obj.Start));
            
            return fromStartToEnd - pauses;
        }

        //public static TimeSpan CountBalance(List<DayTypeEntity> dayTypes, List<ShiftPauseEntity> workRecords)
        //{
        //    var balance = TimeSpan.Zero;
        //    var shifts = workRecords
        //        .Where(wr => wr.End.HasValue)
        //        .GroupBy(wr => wr.Start.Day);
        //    foreach (var shift in shifts)
        //    {
        //        var firstRecord = shift.First();
        //        var dayType = dayTypes.FirstOrDefault(dt => dt.DayTypeID == firstRecord.DayTypeID);
        //        if (dayType == null) 
        //            continue;

        //        var dayTotalHours = shift.Aggregate(TimeSpan.Zero, (sum, obj) => sum + ((DateTime)obj.End - obj.Start));
        //        var dayActuallyHours = dayTotalHours < dayType.ShiftTimeWithoutBreak ?
        //            dayTotalHours :
        //            (dayTotalHours - dayType.BreakTime);
        //        var requiredHours = dayTotalHours < dayType.ShiftTimeWithoutBreak ?
        //            dayType.ShiftTimeWithoutBreak :
        //            (dayType.ShiftTime - dayType.BreakTime);
        //        var dayBalance = dayActuallyHours - requiredHours;
        //        balance += dayBalance;
        //    }

        //    return balance;
        //}
    }
}
