using FluentResults;
using Hosys.Application.Data.Outputs.User;

namespace Hosys.Application.Interfaces.UseCases
{
    public interface IUserUseCases
    {
        Task<Result> CreateUser(CreateUserDTO userDto);
    }
}