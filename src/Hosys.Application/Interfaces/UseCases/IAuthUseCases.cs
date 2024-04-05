using FluentResults;
using Hosys.Application.Data.Outputs.Auth;

namespace Hosys.Application.Interfaces.UseCases
{
    public interface IAuthUseCases
    {
        Task<Result> SignUp(CreateUserDTO userDto);
        Task<Result<AuthTokenDTO>> SignIn(AuthSignInDTO userDto);
    }
}