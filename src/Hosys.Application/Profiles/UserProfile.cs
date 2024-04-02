using AutoMapper;
using Hosys.Application.Data.Outputs.Auth;
using Hosys.Domain.Models.User;

namespace Hosys.Application.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, ReadUserDTO>();
            CreateMap<CreateUserDTO, User>();
            CreateMap<UpdateUserDTO, User>();
        }
    }
}