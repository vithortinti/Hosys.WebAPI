using FluentResults;

namespace Hosys.Application.Interfaces.UseCases
{
    public interface IUserUseCases
    {
        Task<Result> DeleteUser(Guid id, string confirmPassword);
    }
}