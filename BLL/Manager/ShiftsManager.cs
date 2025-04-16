using AutoMapper;
using Core.Consts;
using Core.DTO;
using Core.Helper;
using Core.Entity.Shift;
using Core.View.Shift;
using Core.Model.WorkRecord;
using Core.View.Statistics;
using Core.Service;
using Core.Interface.Repository;

namespace BLL.Manager
{
    public class ShiftsManager
    {
        private readonly IShiftRepository _shiftRepository;
        private readonly IDayTypeRepository _dayTypeRepository;
        private readonly ValidationService _validationService;
        private readonly IMapper _mapper;
        public ShiftsManager(IShiftRepository shiftRepository,
            IDayTypeRepository dayTypeRepository,
            ValidationService validationService, 
            IMapper mapper)
        {
            _shiftRepository = shiftRepository;
            _dayTypeRepository = dayTypeRepository;
            _validationService = validationService;
            _mapper = mapper;
        }

        public async Task<CommonContentDTO<CurrentBalanceView>> GetCurrentBalanceAsync(long userID, DateTimeView requestView)
        {
            var res = new CommonContentDTO<CurrentBalanceView>();

            var shifts = await _shiftRepository.GetAsync(userID, requestView.Date.Year, requestView.Date.Month);
            var dayTypes = await _dayTypeRepository.GetDayTypesByUserAsync(userID);

            res.Status = CustomResponseStatus.OK;
            res.View.Content = StatisticsHelper.GetCurrentBalance(dayTypes, shifts);
            return res;
        }

        public async Task<CommonContentDTO<GetShiftView>> GetDailyDataAsync (long userID, DateTimeView requestView)
        {
            var res = new CommonContentDTO<GetShiftView>();

            var shift = await _shiftRepository.GetAsync(userID, requestView.Date);
            if (shift == null)
            {
                res.Status = CustomResponseStatus.NoContent;
                return res;
            }

            var view = _mapper.Map<GetShiftView>(shift);
            var dayType = shift.DayTypeID.HasValue
                ? await _dayTypeRepository.GetAsync(shift.DayTypeID.Value)
                : null;

            if (dayType != null)
            {
                view.DayType = _mapper.Map<ShiftDayTypeView>(dayType);
                view.Statistics = StatisticsHelper.GetStatistics(dayType, shift);
            }

            res.Status = CustomResponseStatus.OK;
            res.View.Content = view;
            return res;
        }

        public async Task<CommonListDTO<GetShiftView>> GetMonthlyDataAsync(long userID, int year, int month)
        {
            var res = new CommonListDTO<GetShiftView>();

            var shifts = await _shiftRepository.GetAsync(userID, year, month);
            var dayTypes = await _dayTypeRepository.GetDayTypesByUserAsync(userID);
            var dayTypeIds = dayTypes.Select(dt => dt.DayTypeID).ToList();

            if (shifts.Any(sh => sh.DayTypeID.HasValue && !dayTypeIds.Contains(sh.DayTypeID.Value)))
            {
                res.Status = CustomResponseStatus.BadRequest;
                res.View.Message = DayTypeConsts.NotExistingError;
                return res;
            }

            var views = new List<GetShiftView>();
            
            foreach (var shift in shifts)
            {
                var view = _mapper.Map<GetShiftView>(shift);
                var dayType = shift.DayTypeID.HasValue
                    ? dayTypes.Find(dt => dt.DayTypeID == shift.DayTypeID.Value)
                    : null;

                if (dayType != null)
                {
                    view.DayType = _mapper.Map<ShiftDayTypeView>(dayType);
                    view.Statistics = StatisticsHelper.GetStatistics(dayType, shift);
                }
                views.Add(view);
            }

            res.Status = CustomResponseStatus.OK;
            res.View.Content = views;
            return res;
        }

        public async Task<CommonDTO> CreateAsync(long userID, ShiftView requestView)
        {
            var res = new CommonDTO();

            var entity = await _shiftRepository.GetAsync(userID, requestView.Date);
            if (entity != null)
            {
                res.Status = CustomResponseStatus.BadRequest;
                res.View.Message = ShiftConsts.AlreadyExistsError;
                return res;
            }

            entity = _mapper.Map<ShiftEntity>(requestView);
            entity.UserID = userID;

            var validationResultDto = _validationService.ValidateShift(entity);
            if (!validationResultDto.IsValid)
            {
                res.Status = CustomResponseStatus.BadRequest;
                res.View.Message = validationResultDto.ErrorMessage;
                return res;
            }

            await _shiftRepository.CreateAsync(entity);

            res.Status = CustomResponseStatus.OK;
            res.View.Message = ShiftConsts.UpdateSuccess;
            return res;
        }

        public async Task<CommonDTO> UpdateAsync(long userID, long shiftID, ShiftView requestView)
        {
            var res = new CommonDTO();

            var entity = await _shiftRepository.GetByIDAsync(shiftID);
            if (entity == null)
            {
                res.Status = CustomResponseStatus.BadRequest;
                res.View.Message = ShiftConsts.NotFoundError;
                return res;
            }

            if (entity.UserID != userID)
            {
                res.Status = CustomResponseStatus.BadRequest;
                res.View.Message = GeneralConsts.PermissionsError;
                return res;
            }

            _mapper.Map(entity, requestView);

            var validationResultDto = _validationService.ValidateShift(entity);
            if (!validationResultDto.IsValid)
            {
                res.Status = CustomResponseStatus.BadRequest;
                res.View.Message = validationResultDto.ErrorMessage;
                return res;
            }

            await _shiftRepository.UpdateAsync(entity);

            res.Status = CustomResponseStatus.OK;
            res.View.Message = ShiftConsts.UpdateSuccess;
            return res;
        }

        public async Task<CommonDTO> StartShiftAsync(long userID, StartShiftView requestView)
        {
            var res = new CommonDTO();

            var entity = await _shiftRepository.GetAsync(userID, requestView.Date);
            if (entity != null)
            {
                res.Status = CustomResponseStatus.BadRequest;
                res.View.Message = ShiftConsts.ShiftAlreadyHasBeenStarted;
                return res;
            }

            await _shiftRepository.CreateAsync(new ShiftEntity
            {
                UserID = userID,
                DayTypeID = requestView.DayTypeID,
                ShiftDate = requestView.Date.ToUniversalTime(),
                Start = requestView.Date.ToUniversalTime(),
                Stop = null,
                ShiftPauses = new List<ShiftPauseEntity>()
            });

            res.Status = CustomResponseStatus.OK;
            res.View.Message = ShiftConsts.ShiftHasBeenStarted;
            return res;
        }

        public async Task<CommonDTO> StopShiftAsync(long userID, long shiftID, DateTimeView requestView)
        {
            var res = new CommonDTO();

            var entity = await _shiftRepository.GetByIDAsync(shiftID);
            if (entity == null)
            {
                res.Status = CustomResponseStatus.BadRequest;
                res.View.Message = ShiftConsts.ShiftHasNotBeenStarted;
                return res;
            }

            if (entity.UserID != userID)
            {
                res.Status = CustomResponseStatus.BadRequest;
                res.View.Message = GeneralConsts.PermissionsError;
                return res;
            }

            entity.Stop = requestView.Date;

            if (entity.ShiftPauses.Any())
            {
                var lastPause = entity.ShiftPauses.Last();
                lastPause.Stop ??= requestView.Date;
            }

            var validationResultDto = _validationService.ValidateShift(entity);
            if (!validationResultDto.IsValid)
            {
                res.Status = CustomResponseStatus.BadRequest;
                res.View.Message = validationResultDto.ErrorMessage;
                return res;
            }

            await _shiftRepository.UpdateAsync(entity);

            res.Status = CustomResponseStatus.OK;
            res.View.Message = ShiftConsts.ShiftHasBeenStopped;
            return res;
        }

        public async Task<CommonDTO> StartPauseAsync(long userID, long shiftID, DateTimeView requestView)
        {
            var res = new CommonDTO();

            var entity = await _shiftRepository.GetByIDAsync(shiftID);
            if (entity == null)
            {
                res.Status = CustomResponseStatus.BadRequest;
                res.View.Message = ShiftConsts.ShiftHasNotBeenStarted;
                return res;
            }

            if (entity.UserID != userID)
            {
                res.Status = CustomResponseStatus.BadRequest;
                res.View.Message = GeneralConsts.PermissionsError;
                return res;
            }

            if (entity.Stop.HasValue)
            {
                res.Status = CustomResponseStatus.BadRequest;
                res.View.Message = ShiftConsts.ShiftHasBeenStopped;
                return res;
            }

            entity.ShiftPauses.Add(new ShiftPauseEntity { Start = requestView.Date, Stop = null });

            var validationResultDto = _validationService.ValidateShift(entity);
            if (!validationResultDto.IsValid)
            {
                res.Status = CustomResponseStatus.BadRequest;
                res.View.Message = validationResultDto.ErrorMessage;
                return res;
            }

            await _shiftRepository.UpdateAsync(entity);

            res.Status = CustomResponseStatus.OK;
            res.View.Message = ShiftConsts.PauseHasBeenStarted;
            return res;
        }

        public async Task<CommonDTO> StopPauseAsync(long userID, long shiftID, DateTimeView requestView)
        {
            var res = new CommonDTO();

            var entity = await _shiftRepository.GetByIDAsync(shiftID);
            if (entity == null)
            {
                res.Status = CustomResponseStatus.BadRequest;
                res.View.Message = ShiftConsts.ShiftHasNotBeenStarted;
                return res;
            }

            if (entity.UserID != userID)
            {
                res.Status = CustomResponseStatus.BadRequest;
                res.View.Message = GeneralConsts.PermissionsError;
                return res;
            }

            if (entity.Stop.HasValue)
            {
                res.Status = CustomResponseStatus.BadRequest;
                res.View.Message = ShiftConsts.ShiftHasBeenStopped;
                return res;
            }

            var lastPause = entity.ShiftPauses.LastOrDefault();
            if (lastPause == null || lastPause.Stop.HasValue)
            {
                res.Status = CustomResponseStatus.BadRequest;
                res.View.Message = ShiftConsts.PauseHasNotBeenStarted;
                return res;
            }

            lastPause.Stop = requestView.Date;

            var validationResultDto = _validationService.ValidateShift(entity);
            if (!validationResultDto.IsValid)
            {
                res.Status = CustomResponseStatus.BadRequest;
                res.View.Message = validationResultDto.ErrorMessage;
                return res;
            }

            await _shiftRepository.UpdateAsync(entity);

            res.Status = CustomResponseStatus.OK;
            res.View.Message = ShiftConsts.PauseHasBeenStopped;
            return res;
        }

        public async Task<CommonDTO> DeleteAsync(long userID, long shiftID)
        {
            var res = new CommonDTO();

            var entity = await _shiftRepository.GetByIDAsync(shiftID);
            if (entity == null || entity.UserID != userID)
            {
                res.Status = CustomResponseStatus.BadRequest;
                res.View.Message = GeneralConsts.PermissionsError;
                return res;
            }

            await _shiftRepository.DeleteAsync(entity);

            res.Status = CustomResponseStatus.OK;
            res.View.Message = ShiftConsts.DeleteSuccess;
            return res;
        }
    }
}
