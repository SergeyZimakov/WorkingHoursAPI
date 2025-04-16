using AutoMapper;
using Core.Consts;
using Core.DTO;
using Core.Entity.DayType;
using Core.Interface.Repository;
using Core.Service;
using Core.View.DayType;
using DAL;
using DAL.Repoitory;

namespace BLL.Manager
{
    public class DayTypeManager
    {
        private readonly IDayTypeRepository _dayTypeRepository;
        private readonly ValidationService _validationService;
        private readonly IMapper _mapper;
        public DayTypeManager(IDayTypeRepository dayTypeRepository,
            ValidationService validationService,
            IMapper mapper)
        {
            _dayTypeRepository = dayTypeRepository;
            _validationService = validationService;
            _mapper = mapper;
        }

        public async Task<CommonListDTO<GetDayTypeView>> GetByUserAsync(long userID)
        {
            var res = new CommonListDTO<GetDayTypeView>();
            var dayTypesModels = await _dayTypeRepository.GetDayTypesByUserAsync(userID);
            var views = new List<GetDayTypeView>();
            foreach (var dayTypesModel in dayTypesModels)
            {
                var view = _mapper.Map<GetDayTypeView>(dayTypesModel);
                view.AdditionalHours = _mapper.Map<List<AdditionalHoursView>>(dayTypesModel.AdditionalHours);
                views.Add(view);
            }
            res.Status = CustomResponseStatus.OK;
            res.View.Content = views;
            return res;
        }

        public async Task<CommonDTO> CreateAsync(long userID, DayTypeView requestView)
        {
            var res = new CommonDTO();

            var validationResultDto = _validationService.ValidateDayType(requestView);
            if (!validationResultDto.IsValid)
            {
                res.Status = CustomResponseStatus.BadRequest;
                res.View.Message = validationResultDto.ErrorMessage;
                return res;
            }
            
            var dayTypeModel = _mapper.Map<DayTypeEntity>(requestView);
            dayTypeModel.AdditionalHours = _mapper.Map<List<AdditionalHoursEntity>>(requestView.AdditionalHours);
            dayTypeModel.UserID = userID;
            
            await _dayTypeRepository.CreateAsync(dayTypeModel);

            res.Status = CustomResponseStatus.OK;
            res.View.Message = DayTypeConsts.AddSuccess;
            return res;
        }

        public async Task<CommonDTO> UpdateAsync(long userID, long dayTypeID, DayTypeView requestView)
        {
            var res = new CommonDTO();

            var dayTypeModel = await _dayTypeRepository.GetAsync(dayTypeID);
            if (dayTypeModel == null)
            {
                res.Status = CustomResponseStatus.BadRequest;
                res.View.Message = DayTypeConsts.NotFoundError;
                return res;
            }

            var validationResultDto = _validationService.ValidateDayType(requestView);
            if (!validationResultDto.IsValid)
            {
                res.Status = CustomResponseStatus.BadRequest;
                res.View.Message = validationResultDto.ErrorMessage;
                return res;
            }

            if (userID != dayTypeModel.UserID)
            {
                res.Status = CustomResponseStatus.BadRequest;
                res.View.Message = DayTypeConsts.UserError;
                return res;
            }

            _mapper.Map(requestView, dayTypeModel);
            dayTypeModel.AdditionalHours = _mapper.Map<List<AdditionalHoursEntity>>(requestView.AdditionalHours);
            
            await _dayTypeRepository.UpdateAsync(dayTypeModel);

            res.Status = CustomResponseStatus.OK;
            res.View.Message = DayTypeConsts.UpdateSuccess;
            return res;
        }

        public async Task<CommonDTO> DeleteAsync(long dayTypeID)
        {
            var res = new CommonDTO();

            var dayType = await _dayTypeRepository.GetAsync(dayTypeID);
            if (dayType == null)
            {
                res.Status = CustomResponseStatus.BadRequest;
                res.View.Message = DayTypeConsts.NotFoundError;
                return res;
            }

            await _dayTypeRepository.DeleteAsync(dayType);

            res.Status = CustomResponseStatus.OK;
            res.View.Message = DayTypeConsts.DeleteSuccess;
            return res;
        }
    }
}
