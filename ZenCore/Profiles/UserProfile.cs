using AutoMapper;

namespace ZenCore.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Entities.User, Models.UserDto>();
            CreateMap<Models.UserDto, Entities.User>();
            CreateMap<Models.UserForUpdateDto, Entities.User>();
        }
    }
}
