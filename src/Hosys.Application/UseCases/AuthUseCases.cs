using System.Text.RegularExpressions;
using AutoMapper;
using FluentResults;
using Hosys.Application.Data.Outputs.Auth;
using Hosys.Application.Interfaces.UseCases;
using Hosys.Domain.Models.User;
using Hosys.Services.Jwt.Handle;
using Hosys.Identity.Interfaces;
using Hosys.Security.Interfaces;

namespace Hosys.Application.UseCases
{
    public class AuthUseCases(
        IIdentityManager identityManager,
        IIdentityValidator identityValidator,
        ITextSecurityAnalyzer textSecurityAnalyzer,
        JwtService jwtService, 
        IMapper mapper) : IAuthUseCases
    {
        private readonly IIdentityManager _identityManager = identityManager;
        private readonly IIdentityValidator _identityValidator = identityValidator;
        private readonly ITextSecurityAnalyzer _textSecurityAnalyzer = textSecurityAnalyzer;
        private readonly JwtService _jwtService = jwtService;
        private readonly IMapper _mapper = mapper;

        public async Task<Result> SignUp(CreateUserDTO userDto)
        {
            // Validate DTO
            if (userDto == null)
                return Result.Fail("User cannot be null.");
            if (string.IsNullOrEmpty(userDto?.Name))
                return Result.Fail("Name cannot be null or empty.");
            if (string.IsNullOrEmpty(userDto?.LastName))
                return Result.Fail("Last Name cannot be null or empty.");
            if (string.IsNullOrEmpty(userDto?.NickName))
                return Result.Fail("NickName cannot be null or empty.");
            if (string.IsNullOrEmpty(userDto?.Email))
                return Result.Fail("Email cannot be null or empty.");
            if (string.IsNullOrEmpty(userDto?.Password))
                return Result.Fail("Password cannot be null or empty.");

            // Validate if the text contains a script tag
            if (_textSecurityAnalyzer.HasScriptTag(userDto.Name))
                return Result.Fail("The name constains a script tag.");
            else if (_textSecurityAnalyzer.HasScriptTag(userDto.LastName))
                return Result.Fail("The last name constains a script tag.");
            else if (_textSecurityAnalyzer.HasScriptTag(userDto.NickName))
                return Result.Fail("The nickname constains a script tag.");
            else if (_textSecurityAnalyzer.HasScriptTag(userDto.Email))
                return Result.Fail("The email constains a script tag.");

            // Check if email already exists
            var emailExists = (await _identityValidator.EmailExists(userDto.Email)).Value;
            if (emailExists)
                return Result.Fail($"The email {userDto.Email} is already in use.");

            // Check if nickname already exists
            var nicknameExists = (await _identityValidator.NicknameExists(userDto.NickName)).Value;
            if (nicknameExists)
                return Result.Fail($"The nickname {userDto.NickName} already exists.");

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
            Result result = await _identityManager.CreateUser(user, userDto.Password);
            if (result.IsFailed)
                return Result.Fail(result.Errors);

            return Result.Ok();
        }

        public async Task<Result<AuthTokenDTO>> SignIn(AuthSignInDTO userDto)
        {
            // Validate DTO
            if (userDto == null)
                return Result.Fail("User cannot be null.");
            else if (string.IsNullOrEmpty(userDto.NickName))
                return Result.Fail("User name cannot be empty.");
            else if (string.IsNullOrEmpty(userDto.Password))
                return Result.Fail("Password cannot be empty.");

            // Get user by nickname
            var user = await _identityManager.GetUserByNickname(userDto.NickName);
            if (user.IsFailed)
                return Result.Fail(user.Errors);

            // Validate password
            var checkResult = await _identityValidator.CheckUser(user.Value, userDto.Password);
            if (checkResult.IsSuccess)
            {
                if (checkResult.Value)
                {
                    // Generate token
                    var token = _jwtService.GenerateToken(user.Value);
                    return Result.Ok(_mapper.Map<AuthTokenDTO>(token));
                }
            }

            return Result.Fail(checkResult.Errors);
        }
    }
}