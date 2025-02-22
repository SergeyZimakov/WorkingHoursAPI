using AutoMapper;
using Core.Entity.DayType;
using Core.Entity.Shift;
using Core.Model.WorkRecord;
using Core.View.Shift;

namespace WorkingHoursAPI.Mapper
{
    public class ShiftMapper : Profile
    {
        public ShiftMapper() 
        {
            CreateMap<ShiftEntity, GetShiftView>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.ShiftDate))
                .ForMember(dest => dest.StartStop, opt => opt.MapFrom(src => new StartStopView
                {
                    Start = src.Start,
                    Stop = src.Stop,
                }))
                .ForMember(dest => dest.Pauses, opt => opt.MapFrom(src => src.ShiftPauses));

            CreateMap<ShiftPauseEntity, StartStopView>();
            CreateMap<StartStopView, ShiftPauseEntity>()
                .ForMember(dest => dest.Start, opt => opt.MapFrom(src => src.Start.ToUniversalTime()))
                .ForMember(dest => dest.Stop, opt => opt.MapFrom(src => src.Stop.HasValue ? src.Stop.Value.ToUniversalTime() : (DateTime?)null));

            CreateMap<DayTypeEntity, ShiftDayTypeView>();

            CreateMap<ShiftView, ShiftEntity>()
                .ForMember(dest => dest.ShiftDate, opt => opt.MapFrom(src => src.Date.ToUniversalTime()))
                .ForMember(dest => dest.Start, opt => opt.MapFrom(src => src.StartStop.Start.ToUniversalTime()))
                .ForMember(dest => dest.Stop, opt => opt.MapFrom(src => src.StartStop.Stop.HasValue ? src.StartStop.Stop.Value.ToUniversalTime() : (DateTime?)null))
                .ForMember(dest => dest.ShiftPauses, opt => opt.MapFrom(src => src.Pauses));
        }
    }
}
