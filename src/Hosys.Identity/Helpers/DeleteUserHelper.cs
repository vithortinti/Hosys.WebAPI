using FluentResults;
using Hosys.Domain.Interfaces.Files;
using Hosys.Domain.Interfaces.User;
using Hosys.Domain.Models.User;

namespace Hosys.Identity.Helpers
{
    public class DeleteUserHelper(
        IUserRepository userRepository,
        IUserRecoveryRepository userRecoveryRepository,
        IFileHistoryRepository fileHistoryRepository)
    {
        public async Task<Result> DeleteUser(User user)
        {
            // First, delete the user recovery
            Result deletedUserRecovery = await userRecoveryRepository.Delete(user.Id);
            if (deletedUserRecovery.IsFailed)
                return Result.Fail(deletedUserRecovery.Errors);

            // Then, delete the user's files
            Result deletedFiles = await fileHistoryRepository.DeleteAllFromUser(user.Id);
            if (deletedFiles.IsFailed)
                return Result.Fail(deletedFiles.Errors);
            
            // Finally, delete the user
            Result deletedUser = await userRepository.Delete(user.Id);
            if (deletedUser.IsFailed)
                return Result.Fail(deletedUser.Errors);

            return Result.Ok();
        }
    }
}