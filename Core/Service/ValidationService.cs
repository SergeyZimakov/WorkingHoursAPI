using Core.Consts;
using Core.DTO;
using Core.Entity.Shift;
using Core.View.DayType;

namespace Core.Service
{
    public class ValidationService
    {
        public ValidationService() { }
        public ValidationResultDTO ValidateShift(ShiftEntity shift)
        {
            var validationResult = new ValidationResultDTO();
            if (shift.Start >= shift.Stop)
            {
                validationResult.ErrorMessage = ShiftConsts.TimeError;
                return validationResult;
            }

            if (shift.ShiftPauses.Any())
            {
                if (IsOverlapped(shift.ShiftPauses.Select(p => (p.Start, p.Stop)).ToList()))
                {
                    validationResult.ErrorMessage = ShiftConsts.OverlappingError;
                    return validationResult;
                }

                var lastPause = shift.ShiftPauses.Last();

                if (shift.Stop.HasValue && !lastPause.Stop.HasValue)
                {
                    validationResult.ErrorMessage = ShiftConsts.NotStoppedPauseError;
                    return validationResult;
                }

                if (shift.ShiftPauses.SkipLast(1).Any(p => !p.Stop.HasValue))
                {
                    validationResult.ErrorMessage = ShiftConsts.NotLastPauseActiveError;
                    return validationResult;
                }

                if (shift.ShiftPauses.Any(p => p.Start >= p.Stop))
                {
                    validationResult.ErrorMessage = ShiftConsts.TimeError;
                    return validationResult;
                }

                if (shift.ShiftPauses.Any(p => p.Start < shift.Start))
                {
                    validationResult.ErrorMessage = ShiftConsts.PauseBeforeStartError;
                    return validationResult;
                }

                if (shift.ShiftPauses.Any(p => p.Stop > shift.Stop))
                {
                    validationResult.ErrorMessage = ShiftConsts.PauseAfterStopError;
                    return validationResult;
                }
            }

            validationResult.IsValid = true;
            return validationResult;
        }

        public ValidationResultDTO ValidateDayType(DayTypeView view)
        {
            var validationResult = new ValidationResultDTO();
            if (view.AdditionalHours.Select(ah => ah.Order).GroupBy(o => o).Any(g => g.Count() > 1))
            {
                validationResult.ErrorMessage = DayTypeConsts.AdditionaHoursOrderError;
                return validationResult;
            }
            if (view.BreakTime > view.ShiftTime || view.BreakTime > view.ShiftTimeWithoutBreak)
            {
                validationResult.ErrorMessage = DayTypeConsts.BreakTimeError;
                return validationResult;
            }

            validationResult.IsValid = true;
            return validationResult;
        }

        private bool IsOverlapped(List<(DateTime Start, DateTime? Stop)> hours)
        {
            return hours
                .OrderBy(wr => wr.Start)
                .Zip(hours.Skip(1), (current, next) => new { Current = current, Next = next })
                .Any(pair => pair.Current.Stop.HasValue && pair.Current.Stop > pair.Next.Start);
        }

    }
}
