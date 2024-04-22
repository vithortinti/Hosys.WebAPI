using System.Security.Cryptography;
using System.Text;
using FluentResults;
using Hosys.Domain.Interfaces.User;
using Hosys.Domain.Models.User;
using Hosys.Identity.Helpers;
using Hosys.Identity.Interfaces;
using Hosys.Security.Interfaces;

namespace Hosys.Identity
{
    public class IdentityManager(
        IUserRepository userRepository,
        IUserRecoveryRepository userRecoveryRepository,
        DeleteUserHelper deleteUserHelper,
        IHash hash
        ) : IIdentityManager
    {
        public async Task<Result> CreateUser(User user, string password)
        {
            // Check if the user is the first user in the system
            if ((await userRepository.Count()).Value == 0)
            {
                user.Role = "ADMIN";
            }

            // First, create the user
            Result<User> createdUser = await userRepository.Create(user, await hash.HashAsync(password));
            if (createdUser.IsFailed)
                return Result.Fail(createdUser.Errors);

            // Then, create the user recovery
            string recoveryKey;
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] recoveryKeyBytes = sha256.ComputeHash(
                    Encoding.UTF8.GetBytes($"{user.Id}{DateTime.Now.Millisecond}{Guid.NewGuid()}_{DateTime.Now}")
                );

                recoveryKey = Convert.ToBase64String(recoveryKeyBytes);
                recoveryKey = recoveryKey.Remove(recoveryKey.Length - 2, 2)
                    .ToUpper();
            }
            _ = await userRecoveryRepository.Create(createdUser.Value.Id, recoveryKey);
            
            return Result.Ok();
        }

        public async Task<Result> DeleteUser(Guid id)
        {
            Result<User> user = await userRepository.Get(id);
            if (user.IsFailed)
                return Result.Fail(user.Errors);
            return await deleteUserHelper.DeleteUser(user.Value);
        }

        public async Task<Result<string>> GetRecoveryKey(Guid userId)
        {
            Result<UserRecovery> userRecovery = await userRecoveryRepository.Get(userId);
            if (userRecovery.IsFailed)
                return Result.Fail(userRecovery.Errors);

            return Result.Ok(userRecovery.Value.RecoveryKey);
        }

        public Task<Result<User>> GetUserById(Guid userId)
        {
            return userRepository.Get(userId);
        }

        public async Task<Result<User>> GetUserByNickname(string nickname)
        {
            Result<User> user = await userRepository.GetByNickname(nickname);
            if (user.IsFailed)
                return Result.Fail(["User not found.", ..user.Errors.Select(x => x.Message)]);

            return Result.Ok(user.Value);
        }

        public async Task<Result> UpdateUser(User user)
        {
            return await userRepository.Update(user);
        }
    }
}