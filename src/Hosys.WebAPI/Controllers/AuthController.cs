using Hosys.Application.Data.Outputs.Auth;
using Hosys.Application.Interfaces.UseCases;
using Hosys.Logger;
using Microsoft.AspNetCore.Mvc;

namespace Hosys.WebAPI.Controllers
{
    [ApiController]
    [Route(AppConfiguration.API_ROUTE + "[controller]")]
    public class AuthController(IAuthUseCases userUseCases, IAppLogger<AuthController> logger) : ControllerBase
    {
        private readonly IAuthUseCases _userUseCases = userUseCases;

        /// <summary>
        /// Sign in a user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("signin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SignIn([FromBody] AuthSignInDTO user)
        {
            try
            {
                var result = await _userUseCases.SignIn(user);
                if (result.IsFailed)
                {
                    logger.LogWarning($"Failed to sign in user {user.NickName}.", result.Errors.Select(e => e.Message).ToList());
                    return BadRequest(new { message = result.Errors[0].Message });
                }

                logger.LogInformation($"User {user.NickName} signed in successfully.");
                return Ok(result.Value);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                return BadRequest(new { message = "An unexpected error occured." });
            }
        }

        /// <summary>
        /// Sign up a new user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("signup")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SignUp([FromBody] CreateUserDTO user)
        {
            try
            {
                var result = await _userUseCases.SignUp(user);
                if (result.IsFailed)
                {
                    logger.LogWarning($"Failed to create the user {user.NickName}.", result.Errors.Select(e => e.Message).ToList());
                    return BadRequest(new { message = result.Errors[0].Message });
                }
                
                logger.LogInformation($"User {user.NickName} created successfully.");
                return Ok(new { message = "User created successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                return BadRequest(new { message = "An unexpected error occured." });
            }
        }
    }
}