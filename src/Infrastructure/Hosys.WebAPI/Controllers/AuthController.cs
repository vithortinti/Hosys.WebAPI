using Hosys.Application.Data.Outputs.User;
using Hosys.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hosys.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] CreateUserDTO user)
        {
            try
            {
                var result = await _userService.CreateUser(user);
                if (result.IsFailed)
                    return BadRequest(new { message = result.Errors[0].Message });
                
                return Ok(result.Value);
            }
            catch
            {
                return BadRequest(new { message = "An unexpected error occured." });
            }
        }
    }
}