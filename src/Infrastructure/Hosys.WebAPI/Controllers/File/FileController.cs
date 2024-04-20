using FluentResults;
using Hosys.Application.Data.Outputs.File;
using Hosys.Application.Interfaces.UseCases;
using Hosys.Application.Ports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hosys.WebAPI.Controllers.File
{
    [ApiController]
    [Route(AppConfiguration.API_ROUTE + "[controller]")]
    [Authorize]
    public class FileController(
        IFileUseCases fileUseCases, 
        IPdfUseCases pdfUseCases,
        IFileHistoryUseCases fileHistoryUseCases
        ) : ControllerBase
    {

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Authorize]
        public async Task<IActionResult> GetUserFiles(int skip = 0, int take = 5)
        {
            try
            {
                // Get the user files
                Result<IEnumerable<ReadFileHistoryDTO>> result = 
                    await fileHistoryUseCases.GetByUserId(
                        Guid.Parse(User.FindFirst("Id")!.Value), 
                        skip, 
                        take
                        );
                if (result.IsFailed)
                    return BadRequest(new { message = result.Errors[0].Message });

                return Ok(result.Value);
            }
            catch
            {
                return BadRequest(new { message = "An unexpected error occured." });
            }
        }

        [HttpPost("pdftoimage")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Authorize]
        public async Task<IActionResult> ConvertPdfToImage([FromForm] FileInput fileInput)
        {
            try
            {
                // Convert the PDF file to images
                var result = await pdfUseCases.ConvertToImage(fileInput.File!);
                if (result.IsFailed)
                    return BadRequest(new { message = result.Errors[0].Message });

                _ = await fileHistoryUseCases.Create(new CreateFileHistoryDTO
                {
                    UserId = Guid.Parse(User.FindFirst("Id")!.Value),
                    FileName = result.Value.Name,
                    ContentType = result.Value.ContentType,
                    FilePath = result.Value.Path,
                    FileExtension = result.Value.Extension,
                    CreatedAt = DateTime.Now
                }, Guid.Parse(User.FindFirst("Id")!.Value));

                return File(result.Value.FileStream, result.Value.ContentType, result.Value.Name);
            }
            catch
            {
                return BadRequest(new { message = "An unexpected error occured." });
            }
        }

        [HttpPost("corrupt")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Authorize]
        public async Task<IActionResult> CorruptFile([FromForm] FileInput file)
        {
            try
            {
                Result<FileOutput> result = await fileUseCases.CorruptFile(file.File!);
                if (result.IsFailed)
                    return BadRequest(new { message = result.Errors[0].Message });

                return File(result.Value.FileStream, result.Value.ContentType, result.Value.Name);
            }
            catch
            {
                return BadRequest(new { message = "An unexpected error occured." });
            }
        }

        [HttpGet("download/{fileId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Authorize]
        public async Task<IActionResult> DownloadFile(Guid fileId)
        {
            try
            {
                Result<FileOutput> result = await fileHistoryUseCases.GetFileStream(
                    Guid.Parse(User.FindFirst("Id")!.Value), 
                    fileId
                    );
                if (result.IsFailed)
                    return BadRequest(new { message = result.Errors[0].Message });

                return File(result.Value.FileStream, result.Value.ContentType, result.Value.Name);
            }
            catch
            {
                return StatusCode(500, new { message = "An unexpected error occured." });
            }
        }
    }
}