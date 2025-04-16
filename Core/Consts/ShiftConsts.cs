namespace Core.Consts
{
    public class ShiftConsts
    {
        public const string TimeError = "IncorrectTime";
        public const string NotClosedRecordError = "PreviousRecordIsNotClosed";
        public const string NotFoundError = "ShiftNotFound";
        public const string AlreadyExistsError = "ShiftAlreadyExists";

        public const string AddSuccess = "RecordWasAddedSuccessfully";
        public const string UpdateSuccess = "RecordWasUpdatedSuccessfully";
        public const string DeleteSuccess = "RecordWasDeletedSuccessfully";

        public const string ShiftHasBeenStarted = "ShiftHasBeenStarted";
        public const string ShiftAlreadyHasBeenStarted = "ShiftAlreadyHasBeenStarted";

        public const string ShiftHasBeenStopped = "ShiftHasBeenStopped";
        public const string ShiftHasNotBeenStarted = "ShiftHasNotBeenStarted";

        public const string PauseHasBeenStarted = "PauseHasBeenStarted";
        public const string PauseAlreadyHasBeenStarted = "PauseAlreadyHasBeenStarted";

        public const string PauseHasBeenStopped = "PauseHasBeenStopped";
        public const string PauseHasNotBeenStarted = "PauseHasNotBeenStarted";

        public const string NotLastPauseActiveError = "OnlyLastPauseCanBeActive";

    }
}
