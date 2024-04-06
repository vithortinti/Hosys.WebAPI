using FluentResults;
using Hosys.Application.Interfaces.UseCases;
using Hosys.Domain.Interfaces.Files;
using Hosys.Identity.Interfaces;

namespace Hosys.Application.UseCases
{
    public class UserUseCases(
        IIdentityManager identityManager,
        IIdentityValidator identityValidator,
        IFileHistoryRepository fileHistoryRepository
    ) : IUserUseCases
    {
        public async Task<Result> DeleteUser(Guid id, string confirmPassword)
        {
            // Check the inputs
            if (id == Guid.Empty)
                return Result.Fail("Invalid user id.");

            // Find the user
            var user = await identityManager.GetUserById(id);
            if (user.IsFailed)
                return Result.Fail(user.Errors[0].Message);

            // Check the user's password
            var validator = await identityValidator.CheckUser(user.Value, confirmPassword);
            if (validator.IsFailed)
                return Result.Fail(validator.Errors[0].Message);


            // Remove the user's files from server
            var files = await fileHistoryRepository.GetByUserId(id);
            if (files.IsSuccess)
            {
                if (files?.Value != null)
                {
                    foreach (var file in files.Value)
                    {
                        File.Delete(file.FilePath!);
                    }
                }
            }

            // Finally, delete the user
            return await identityManager.DeleteUser(id);
        }
    }
}