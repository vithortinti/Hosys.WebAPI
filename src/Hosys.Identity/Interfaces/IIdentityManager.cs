using FluentResults;

namespace Hosys.Identity.Interfaces
{
    public interface IIdentityManager
    {
        Task<Result> CreateUser(Domain.Models.User.User user, string password);
        Task<Result> DeleteUser(Guid userId);
        Task<Result> UpdateUser(Domain.Models.User.User user);
        Task<Result<string>> GetRecoveryKey(Guid userId);
        Task<Result<Domain.Models.User.User>> GetUserByNickname(string nickname);
        Task<Result<Domain.Models.User.User>> GetUserById(Guid userId);
        Task<Result<bool>> GetUserStatus(Guid id);
        Task<Result> ActiveUser(Guid id);
        Task<Result> DisableUser(Guid id);
    }
}