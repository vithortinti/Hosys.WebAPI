using AutoMapper;
using Hosys.Application.Data.Outputs.Auth;
using Hosys.Services.Jwt;

namespace Hosys.Application.Profiles
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<Token, AuthTokenDTO>();
        }
    }
}