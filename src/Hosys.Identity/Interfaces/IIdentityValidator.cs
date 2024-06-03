using FluentResults;

namespace Hosys.Identity.Interfaces
{
    public interface IIdentityValidator
    {
        Task<Result<bool>> CheckUser(Domain.Models.User.User user, string password);
        Task<Result<bool>> EmailExists(string email);
        Task<Result<bool>> NicknameExists(string nickname);
    }
}