using Hosys.Application.Interfaces.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hosys.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController(
        IUserUseCases userUseCases
    ) : ControllerBase
    {
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpDelete("delete")]
        [Authorize]
        public async Task<IActionResult> DeleteUser()
        {
            try
            {
                var result = await userUseCases.DeleteUser(Guid.Parse(User.FindFirst("id")!.Value));
                if (result.IsSuccess)
                    return Ok(new { message = "User deleted successfully." });
                else
                    return BadRequest(new { message = result.Errors[0].Message });
            }
            catch
            {
                return BadRequest(new { message = "An unexpected error occured." });
            }
        }
    }
}