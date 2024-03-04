using AutoMapper;
using JWT.DTOs;
using JWT.Models;

namespace JWT.Configuration
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
