using Hosys.Application.Interfaces.UseCases;
using Hosys.Application.Ports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hosys.WebAPI.Controllers.File
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PdfController(IPdfUseCases pdfUseCases) : ControllerBase
    {
        private readonly IPdfUseCases _pdfUseCases = pdfUseCases;

        /// <summary>
        /// Convert a PDF file to images
        /// </summary>
        /// <param name="fileInput"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Authorize(Roles = "ADMIN, USER")]
        public async Task<IActionResult> ConvertToImage([FromForm] FileInput fileInput)
        {
            try
            {
                var result = await _pdfUseCases.ConvertToImage(fileInput.File!);
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