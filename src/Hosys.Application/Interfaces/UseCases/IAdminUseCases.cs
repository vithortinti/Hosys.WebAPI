using FluentResults;
using Hosys.Application.Data.Outputs.Auth;

namespace Hosys.Application.Interfaces.UseCases;

public interface IAdminUseCases
{
    Task<Result> ActiveUser(Guid userId, bool active);
    Task<Result<List<ReadUserDTO>>> GetAllUsers(int skip, int take);
}
