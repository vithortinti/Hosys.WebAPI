using FluentResults;
using Hosys.Domain.Models.User;

namespace Hosys.Domain.Interfaces.User
{
    public interface IUserRecoveryRepository
    {
        Task<Result> Create(Guid userId, string recoveryKey); // C
        Task<Result<UserRecovery>> Get(Guid id); // R
        Task<Result> Update(UserRecovery user); // U
        Task<Result> Delete(Guid id); // D
    }
}