using FluentResults;
using Hosys.Application.Models;

namespace Hosys.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<Result<Token>> GenerateToken(Guid userId, string userName, string role);
    }
}