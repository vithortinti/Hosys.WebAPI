using FluentResults;
using Hosys.Application.Data.Outputs.User;
using Hosys.Application.Models;

namespace Hosys.Application.Interfaces.UseCases
{
    public interface IUserUseCases
    {
        Task<Result<Token>> CreateUser(CreateUserDTO userDto);
    }
}