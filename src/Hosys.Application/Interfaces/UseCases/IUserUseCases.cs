using FluentResults;
using Hosys.Application.Data.Outputs.Auth;

namespace Hosys.Application.Interfaces.UseCases
{
    public interface IUserUseCases
    {
        Task<Result> CreateUser(CreateUserDTO userDto);
        Task<Result<AuthTokenDTO>> SignIn(AuthSignInDTO userDto);
    }
}