using FluentResults;

namespace Hosys.Domain.Interfaces.User
{
    public interface IUserRepository
    {
        Task<Result> Create(Models.User.User user, string password);
        Task<Result<Models.User.User>> Get(Guid id);
        Task<Result<Models.User.User>> GetByEmail(string email);
        Task<Result<Models.User.User>> GetByNickname(string nickname);
        Task<Result> Update(Models.User.User user);
        Task<Result> UpdatePassword(Guid id, string newPassword);
        Task<Result> Delete(Guid id);
    }
}