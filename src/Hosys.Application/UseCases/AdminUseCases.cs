using AutoMapper;
using FluentResults;
using Hosys.Application.Data.Outputs.Auth;
using Hosys.Application.Interfaces.UseCases;
using Hosys.Domain.Interfaces.User;
using Hosys.Identity.Interfaces;

namespace Hosys.Application.UseCases
{
    public class AdminUseCases(
        IIdentityManager identityManager, 
        IUserRepository userRepository, 
        IMapper mapper
        ) : IAdminUseCases
    {
        public async Task<Result> ActiveUser(Guid userId, bool active)
        {
            if (userId == Guid.Empty)
                return Result.Fail("The user id can't be empty.");
            
            if (active)
                return await identityManager.ActiveUser(userId);
            else
                return await identityManager.DisableUser(userId);
        }

        public async Task<Result<List<ReadUserDTO>>> GetAllUsers(int skip, int take)
        {
            var users = await userRepository.GetAll(skip, take);
            if (users.IsFailed)
                return Result.Fail(["Can't get users.", ..users.Errors.Select(e => e.Message).ToList()]);

            return mapper.Map<List<ReadUserDTO>>(users.Value);
        }
    }
}