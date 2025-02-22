using AutoMapper;
using Core.Entity.DayType;
using Core.View.DayType;

namespace WorkingHoursAPI.Mapper
{
    public class DayTypeMapper : Profile
    {
        public DayTypeMapper() 
        {
            CreateMap<DayTypeEntity, GetDayTypeView>();
            CreateMap<DayTypeEntity, DayTypeView>().ReverseMap();
            CreateMap<AdditionalHoursEntity, AdditionalHoursView>().ReverseMap();
        }
    }
}
