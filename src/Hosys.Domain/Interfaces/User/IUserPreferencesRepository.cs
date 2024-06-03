using FluentResults;
using Hosys.Domain.Models.User;

namespace Hosys.Domain.Interfaces.User
{
    public interface IUserPreferencesRepository
    {
        Task<Result> Create(UserPreferences user); // C
        Task<UserPreferences> Get(Guid id); // R
        Task<Result> Update(UserPreferences user); // U
        Task<Result> Delete(Guid id); // D
    }
}