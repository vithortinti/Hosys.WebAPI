using FluentResults;
using Hosys.Domain.Interfaces.User;
using Hosys.Domain.Models.User;
using Hosys.Identity.Interfaces;
using Hosys.Security.Interfaces;

namespace Hosys.Identity
{
    public class IdentityValidator(
        IUserRepository userRepository,
        IHash hash) : IIdentityValidator
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IHash _hash = hash;

        public async Task<Result<bool>> CheckUser(User user, string password)
        {
            return await _userRepository.CheckPassword(user, await _hash.HashAsync(password));
        }

        public async Task<Result<bool>> EmailExists(string email)
        {
            Result<User> user = await _userRepository.GetByEmail(email);
            return Result.Ok(user.IsSuccess);
        }

        public async Task<Result<bool>> NicknameExists(string nickname)
        {
            Result<User> user = await _userRepository.GetByNickname(nickname);
            return Result.Ok(user.IsSuccess);
        }
    }
}