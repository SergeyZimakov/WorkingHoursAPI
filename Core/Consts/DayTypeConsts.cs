namespace Core.Consts
{
    public class DayTypeConsts
    {
        public const string AdditionaHoursOrderError = "Each additional hours record should have unique order number";
        public const string BreakTimeError = "Break time can not be greater then shift time";
        public const string NotFoundError = "Error. The day type was not found in data base";
        public const string UserError = "This user is not avaliable to upate this day type";
        public const string NotExistingError = "Some of declared day types not exist";

        public const string AddSuccess = "Day type was added successfully";
        public const string UpdateSuccess = "Day type was updated successfully";
        public const string DeleteSuccess = "Day type was deleted successfully";

        public const string DefaultDayTypeName = "Standart";
        public const string DefaultDayTypeColor = "transparent";
        public static TimeSpan DefaultDayTypeShiftTime { get; } = TimeSpan.FromHours(9);
        public static TimeSpan DefaultDayTypeBreakTime { get; } = TimeSpan.FromMinutes(30);
        public static TimeSpan DefaultDayTypeShiftTimeWithoutBreak { get; } = TimeSpan.FromHours(6);
    }
}
