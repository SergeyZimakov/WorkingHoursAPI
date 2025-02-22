namespace Core.Consts
{
    public class ShiftConsts
    {
        public const string TimeError = "Start time should be less then end time";
        public const string NotSameDayError = "Start and end time should be at same day";
        public const string OverlappingError = "Overlapping with another records of the day";
        public const string NotClosedRecordError = "The previous record is not closed";
        public const string EndTimeMissingError = "End time is missing";
        public const string UserError = "User permissions error";
        public const string NoDayTypesBalanceError = "Can not count balance without day types definition";
        public const string NotFoundError = "Shift not found";
        public const string AlreadyExistsError = "Shift already exists";

        public const string AddSuccess = "Record was added successfully";
        public const string UpdateSuccess = "Record was updated successfully";
        public const string DeleteSuccess = "Record was deleted successfully";

        public const string StartShiftSuccess = "Shift has been started";
        public const string StartShiftError = "Shift already has been started";

        public const string StopShiftSuccess = "Shift has been stopped";
        public const string StopShiftError = "Shift was not started";

        public const string StartPauseSuccess = "Pause has been started";
        public const string StartPauseError = "Pause already has been started";

        public const string StopPauseSuccess = "Pause has been stopped";
        public const string StopPauseError = "Pause was not started";

        public const string PauseBeforeStartError = "Pause can not start before shift started";
        public const string PauseAfterStopError = "Pause can not stop before shift stopped";
        public const string NotStoppedPauseError = "Can not be active pause when shift stopped";
        public const string NotLastPauseActiveError = "Only last pause can be active";

    }
}
