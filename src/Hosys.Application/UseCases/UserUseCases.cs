using System.Text.RegularExpressions;
using AutoMapper;
using FluentResults;
using Hosys.Application.Data.Outputs.User;
using Hosys.Application.Interfaces.UseCases;
using Hosys.Application.Models;
using Hosys.Domain.Interfaces.User;
using Hosys.Domain.Models.User;

namespace Hosys.Application.UseCases
{
    public class UserUseCases(IUserRepository userRepository, IMapper mapper) : IUserUseCases
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

            // Validate email
            var emailRegex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            if (!emailRegex.IsMatch(userDto.Email))
            {
                // TODO: Add allowed email domains
                return Result.Fail("Invalid email format.");
            }

            // Validate password
            var passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{8,}$");
            if (!passwordRegex.IsMatch(userDto.Password))
            {
                return Result.Fail("Password must have at least 8 characters, one uppercase letter, one lowercase letter and one special character.");
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