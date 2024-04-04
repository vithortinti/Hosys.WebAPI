using FluentResults;
using Hosys.Application.Data.Outputs.File;
using Hosys.Application.Interfaces.UseCases;
using Hosys.Application.Ports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hosys.WebAPI.Controllers.File
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FileController(IFileUseCases fileUseCases) : ControllerBase
    {
        private readonly IFileUseCases _fileUseCases = fileUseCases;

        [HttpPost("corrupt")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Authorize(Roles = "ADMIN, USER")]
        public async Task<IActionResult> CorruptFile([FromForm] FileInput file)
        {
            try
            {
                Result<FileOutput> result = await _fileUseCases.CorruptFile(file.File!);
                if (result.IsFailed)
                    return BadRequest(new { message = result.Errors[0].Message });

                return File(result.Value.FileStream, result.Value.ContentType, result.Value.Name);
            }
            catch
            {
                return BadRequest(new { message = "An unexpected error occured." });
            }
        }
    }
}