using Hosys.Application.Data.Outputs.User;
using Hosys.Application.Interfaces.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Hosys.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IUserUseCases userUseCases) : ControllerBase
    {
        private readonly IUserUseCases _userUseCases = userUseCases;

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
                var result = await _userUseCases.CreateUser(user);
                if (result.IsFailed)
                    return BadRequest(new { message = result.Errors[0].Message });
                
                return Ok(new { message = "User created successfully." });
            }
            catch
            {
                return BadRequest(new { message = "An unexpected error occured." });
            }
        }
    }
}