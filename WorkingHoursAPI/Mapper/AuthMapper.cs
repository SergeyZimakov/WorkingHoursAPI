using AutoMapper;
using Core.DTO;
using Core.Entity.Auth;

namespace WorkingHoursAPI.Mapper
{
    public class AuthMapper : Profile
    {
        public AuthMapper() 
        {
            CreateMap<UserEntity, UserDTO>();
        }
    }
}
