using FluentResults;
using Hosys.Application.Data.Outputs.Auth;
using Hosys.Application.Interfaces.UseCases;
using Hosys.Application.Ports;
using Hosys.Identity.Enums;
using Hosys.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hosys.WebAPI.Controllers;

[Route(AppConfiguration.API_ROUTE + "[controller]")]
[Authorize(Roles = HosysRoles.ADMIN)]
public class AdminController(IAdminUseCases adminUseCases, IAppLogger<AdminController> logger) : ControllerBase
{
    private Guid _userId => Guid.Parse(User.FindFirst("id")!.Value);

    [HttpPost("active/{userId}")]
    [ProducesResponseType(500)]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> ActiveUser(Guid userId, [FromBody] UserStatus userStatus)
    {
        try
        {
            Result result = await adminUseCases.ActiveUser(userId, userStatus.Active);
            if (result.IsFailed)
            {
                await logger.LogWarning("Can't active user", result.Errors.Select(e => e.Message).ToList(), _userId);
                return BadRequest(new { message = result.Errors[0].Message });
            }

            await logger.LogInformation($"User {userId} active status changed to {userStatus.Active}", _userId);
            return NoContent();
        }
        catch(Exception ex)
        {
            await logger.LogError(ex.Message, ex, _userId);
            return StatusCode(500);
        }
    }

    [HttpGet("users")]
    [ProducesResponseType(500)]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetUsers(int skip = 0, int take = 5)
    {
        try
        {
            Result<List<ReadUserDTO>> result = await adminUseCases.GetAllUsers(skip, take);
            if (result.IsFailed)
            {
                await logger.LogWarning("Can't get users from database.", result.Errors.Select(e => e.Message).ToList(), _userId);
                return BadRequest(new { message = result.Errors[0].Message });
            }

            await logger.LogInformation($"{skip} to {take} users are returned from database.", _userId);
            return Ok(result.Value);
        }
        catch(Exception ex)
        {
            await logger.LogError(ex.Message, ex, _userId);
            return StatusCode(500);
        }
    }
}
