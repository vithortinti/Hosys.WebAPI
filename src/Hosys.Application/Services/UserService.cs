using AutoMapper;
using FluentResults;
using Hosys.Application.Data.Outputs.User;
using Hosys.Application.Interfaces.Services;
using Hosys.Application.Models;
using Hosys.Domain.Interfaces.User;
using Hosys.Domain.Models.User;

namespace Hosys.Application.Services
{
    public class UserService(IUserRepository userRepository, IMapper mapper) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<Token>> CreateUser(CreateUserDTO userDto)
        {
            // Validate DTO
            if (userDto == null)
                return Result.Fail("User cannot be null");
            if (string.IsNullOrEmpty(userDto?.Name))
                return Result.Fail("Name cannot be null or empty");
            if (string.IsNullOrEmpty(userDto?.LastName))
                return Result.Fail("Last Name cannot be null or empty");
            if (string.IsNullOrEmpty(userDto?.Nickname))
                return Result.Fail("NickName cannot be null or empty");
            if (string.IsNullOrEmpty(userDto?.Email))
                return Result.Fail("Email cannot be null or empty");
            if (string.IsNullOrEmpty(userDto?.Password))
                return Result.Fail("Password cannot be null or empty");

            // Check if email already exists
            var email = await _userRepository.GetByEmail(userDto.Email);
            if (email.IsSuccess)
            {
                return Result.Fail($"The email {userDto.Email} is already in use.");
            }

            // Check if nickname already exists
            var nickname = await _userRepository.GetByNickname(userDto.Nickname);
            if (nickname.IsSuccess)
            {
                return Result.Fail($"The nickname {userDto.Nickname} already exists.");
            }

            // Map DTO to Domain Model and create user
            var user = _mapper.Map<User>(userDto);
            user.Id = Guid.NewGuid(); // Define the user ID
            Result result = await _userRepository.Create(user, userDto.Password);
            if (result.IsFailed)
                return Result.Fail(result.Errors);

            return Result.Ok();
        }
    }
}