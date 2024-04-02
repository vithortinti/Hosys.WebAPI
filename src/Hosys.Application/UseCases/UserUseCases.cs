using System.Text.RegularExpressions;
using AutoMapper;
using FluentResults;
using Hosys.Application.Data.Outputs.User;
using Hosys.Application.Interfaces.Security.Hash;
using Hosys.Application.Interfaces.Security.Text;
using Hosys.Application.Interfaces.UseCases;
using Hosys.Domain.Interfaces.User;
using Hosys.Domain.Models.User;

namespace Hosys.Application.UseCases
{
    public class UserUseCases(
        IUserRepository userRepository,
        IHash hash, 
        ITextSecurityAnalyzer textSecurityAnalyzer, 
        IMapper mapper) : IUserUseCases
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IHash _hash = hash;
        private readonly ITextSecurityAnalyzer _textSecurityAnalyzer = textSecurityAnalyzer;
        private readonly IMapper _mapper = mapper;

        public async Task<Result> CreateUser(CreateUserDTO userDto)
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

            // Validate if the text contains a script tag
            if (_textSecurityAnalyzer.HasScriptTag(userDto.Name))
                return Result.Fail("The name constains a script tag.");
            else if (_textSecurityAnalyzer.HasScriptTag(userDto.LastName))
                return Result.Fail("The last name constains a script tag.");
            else if (_textSecurityAnalyzer.HasScriptTag(userDto.Nickname))
                return Result.Fail("The nickname constains a script tag.");
            else if (_textSecurityAnalyzer.HasScriptTag(userDto.Email))
                return Result.Fail("The email constains a script tag.");

            // Check if email already exists
            var email = await _userRepository.GetByEmail(userDto.Email);
            if (email.IsSuccess)
                return Result.Fail($"The email {userDto.Email} is already in use.");

            // Check if nickname already exists
            var nickname = await _userRepository.GetByNickname(userDto.Nickname);
            if (nickname.IsSuccess)
                return Result.Fail($"The nickname {userDto.Nickname} already exists.");

            // Validate email format
            var emailRegex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            if (!emailRegex.IsMatch(userDto.Email))
            {
                // TODO: Add allowed email domains
                return Result.Fail("Invalid email format.");
            }

            // Validate password security
            var passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{8,}$");
            if (!passwordRegex.IsMatch(userDto.Password))
                return Result.Fail("Password must have at least 8 characters, one uppercase letter, one lowercase letter and one special character.");

            // Map DTO to Domain Model and create user
            var user = _mapper.Map<User>(userDto);
            Result result = await _userRepository.Create(user, await _hash.HashAsync(userDto.Password));
            if (result.IsFailed)
                return Result.Fail(result.Errors);

            return Result.Ok();
        }

        public async Task<Result<string>> SignIn(SignInUserDTO userDto)
        {
            // Validate DTO
            if (userDto == null)
                return Result.Fail<string>("User cannot be null");
            else if (string.IsNullOrEmpty(userDto.NickName))
                return Result.Fail<string>("User name cannot be empty");
            else if (string.IsNullOrEmpty(userDto.Password))
                return Result.Fail<string>("Password cannot be empty");

            // Get user by nickname
            var userResult = await _userRepository.GetByNickname(userDto.NickName);
            if (userResult.IsFailed)
                return Result.Fail<string>("User not found.");

            // Validate password
            string passwordHash = await _hash.HashAsync(userDto.Password);
            var user = userResult.Value;
            var checkResult = await _userRepository.CheckPassword(user, passwordHash);
            if (checkResult.IsSuccess)
            {
                if (checkResult.Value)
                    return Result.Ok();
            }

            return Result.Fail<string>("Invalid password");
        }
    }
}