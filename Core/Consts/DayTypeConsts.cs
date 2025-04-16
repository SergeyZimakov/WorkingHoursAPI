namespace Core.Consts
{
    public class DayTypeConsts
    {
        public const string AdditionaHoursOrderError = "EachAdditionalHoursRecordShouldHaveUniqueOrderNumber";
        public const string BreakTimeError = "BreakTimeCanNotBeGreaterThenShiftTime";
        public const string NotFoundError = "DayTypeWasNotFoundInDataBase";
        public const string NotExistingError = "SomeOfDeclaredDayTypesDoesNotExist";

        public const string AddSuccess = "DayTypeWasAddedSuccessfully";
        public const string UpdateSuccess = "DayTypeWasUpdatedSuccessfully";
        public const string DeleteSuccess = "DayTypeWasDeletedSuccessfully";

        public const string DefaultDayTypeName = "Standart";
        public const string DefaultDayTypeColor = "transparent";
        public static TimeSpan DefaultDayTypeShiftTime { get; } = TimeSpan.FromHours(9);
        public static TimeSpan DefaultDayTypeBreakTime { get; } = TimeSpan.FromMinutes(30);
        public static TimeSpan DefaultDayTypeShiftTimeWithoutBreak { get; } = TimeSpan.FromHours(6);
    }
}
