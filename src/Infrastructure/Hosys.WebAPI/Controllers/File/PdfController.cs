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
    public class PdfController(IPdfUseCases pdfUseCases, IFileHistoryUseCases fileHistoryUseCases) : ControllerBase
    {
        private readonly IPdfUseCases _pdfUseCases = pdfUseCases;
        private readonly IFileHistoryUseCases _fileHistoryUseCases = fileHistoryUseCases;

        /// <summary>
        /// Convert a PDF file to images
        /// </summary>
        /// <param name="fileInput"></param>
        /// <returns></returns>
        [HttpPost("image")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Authorize]
        public async Task<IActionResult> ConvertToImage([FromForm] FileInput fileInput)
        {
            try
            {
                // Convert the PDF file to images
                var result = await _pdfUseCases.ConvertToImage(fileInput.File!);
                if (result.IsFailed)
                    return BadRequest(new { message = result.Errors[0].Message });
                
                // Save the file history
                _ = _fileHistoryUseCases.Create(new CreateFileHistoryDTO
                {
                    FileName = result.Value.Name,
                    FileExtension = result.Value.Extension,
                    ContentType = result.Value.ContentType,
                    FilePath = result.Value.Path,
                    CreatedAt = DateTime.UtcNow
                }, Guid.Parse(User.FindFirst("id")!.Value));

                return File(result.Value.FileStream, result.Value.ContentType, result.Value.Name);
            }
            catch
            {
                return BadRequest(new { message = "An unexpected error occured." });
            }
        }
    }
}