using FluentResults;
using Hosys.Application.Interfaces.UseCases;
using Hosys.Domain.Interfaces.Files;
using Hosys.Identity.Interfaces;

namespace Hosys.Application.UseCases
{
    public class UserUseCases(
        IIdentityManager identityManager,
        IFileHistoryRepository fileHistoryRepository
    ) : IUserUseCases
    {
        public async Task<Result> DeleteUser(Guid id)
        {
            // Check the inputs
            if (id == Guid.Empty)
                return Result.Fail("Invalid user id.");

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