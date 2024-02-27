using FluentResults;
using Hosys.Application.Data.Outputs.User;
using Hosys.Application.Models;

namespace Hosys.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<Result<Token>> CreateUser(CreateUserDTO userDto);
    }
}