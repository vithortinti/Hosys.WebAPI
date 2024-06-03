using Hosys.Application.Interfaces.UseCases;
using Hosys.Application.Ports;
using Hosys.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hosys.WebAPI.Controllers
{
    [ApiController]
    [Route(AppConfiguration.API_ROUTE + "[controller]")]
    [Authorize]
    public class UserController(
        IUserUseCases userUseCases,
        IAppLogger<UserController> logger
    ) : ControllerBase
    {
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpDelete("delete")]
        [Authorize]
        public async Task<IActionResult> DeleteUser([FromBody] ConfirmPassword confirmPassword)
        {
            try
            {
                var result = await userUseCases.DeleteUser(
                    Guid.Parse(User.FindFirst("id")!.Value), confirmPassword.Password
                    );
                
                if (result.IsFailed)
                {
                    logger.LogWarning(
                        $"Failed to delete user {User.FindFirst("id")!.Value}.",
                        result.Errors.Select(e => e.Message).ToList()
                    );
                    return BadRequest(new { message = result.Errors[0].Message });
                }

                logger.LogInformation($"User {User.FindFirst("id")!.Value} deleted successfully.");
                return NoContent();
            }
            catch (Exception ex)
            {
                await logger.LogError(ex.Message, ex, Guid.Parse(User.FindFirst("id")!.Value));
                return StatusCode(500);
            }
        }
    }
}