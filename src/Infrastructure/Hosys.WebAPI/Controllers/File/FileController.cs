using FluentResults;
using Hosys.Application.Data.Outputs.File;
using Hosys.Application.Interfaces.UseCases;
using Hosys.Application.Ports;
using Hosys.Logger;
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
        IFileHistoryUseCases fileHistoryUseCases,
        IAppLogger<FileController> logger
        ) : ControllerBase
    {
        private Guid userId => Guid.Parse(User.FindFirst("Id")!.Value);

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
                        userId, 
                        skip, 
                        take
                        );
                if (result.IsFailed)
                {
                    await logger.LogWarning(
                        "Failed to retrieve files.", 
                        result.Errors.Select(e => e.Message).ToList(), 
                        userId
                        );
                    return BadRequest(new { message = result.Errors[0].Message });
                }

                await logger.LogInformation("Files retrieved successfully.", userId);
                return Ok(result.Value);
            }
            catch (Exception ex)
            {
                await logger.LogError(ex.Message, ex, userId);
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
                {
                    await logger.LogWarning(
                        "Failed to convert PDF to image.", 
                        result.Errors.Select(e => e.Message).ToList(), 
                        userId
                        );
                    return BadRequest(new { message = result.Errors[0].Message });
                }

                _ = await fileHistoryUseCases.Create(new CreateFileHistoryDTO
                {
                    UserId = userId,
                    FileName = result.Value.Name,
                    ContentType = result.Value.ContentType,
                    FilePath = result.Value.Path,
                    FileExtension = result.Value.Extension,
                    CreatedAt = DateTime.Now
                }, userId);
                
                await logger.LogInformation("PDF converted to image successfully and saved in File History.", userId);
                return File(result.Value.FileStream, result.Value.ContentType, result.Value.Name);
            }
            catch (Exception ex)
            {
                await logger.LogError(ex.Message, ex, userId);
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
                {
                    await logger.LogWarning(
                        "Failed to corrupt file.", 
                        result.Errors.Select(e => e.Message).ToList(), 
                        userId
                        );
                    return BadRequest(new { message = result.Errors[0].Message });
                }

                await logger.LogInformation("File corrupted successfully.", userId);
                return File(result.Value.FileStream, result.Value.ContentType, result.Value.Name);
            }
            catch (Exception ex)
            {
                await logger.LogError(ex.Message, ex, userId);
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
                    userId, 
                    fileId
                    );
                if (result.IsFailed)
                {
                    await logger.LogWarning(
                        $"Failed to download file Id: {fileId}.", 
                        result.Errors.Select(e => e.Message).ToList(), 
                        userId
                        );
                    return BadRequest(new { message = result.Errors[0].Message });
                }

                await logger.LogInformation($"File {fileId} downloaded successfully.", userId);
                return File(result.Value.FileStream, result.Value.ContentType, result.Value.Name);
            }
            catch (Exception ex)
            {
                await logger.LogError(ex.Message, ex, userId);
                return StatusCode(500, new { message = "An unexpected error occured." });
            }
        }

        [HttpDelete("delete/{fileId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize]
        public async Task<IActionResult> DeleteFile(Guid fileId)
        {
            try
            {
                Result result = await fileHistoryUseCases.DeleteFile(
                    userId, 
                    fileId
                    );
                if (result.IsFailed)
                {
                    await logger.LogWarning(
                        $"Failed to delete file Id: {fileId}.", 
                        result.Errors.Select(e => e.Message).ToList(), 
                        userId
                        );
                    return BadRequest(new { message = result.Errors[0].Message });
                }

                await logger.LogInformation("File deleted successfully.", userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                await logger.LogError(ex.Message, ex, userId);
                return StatusCode(500, new { message = "An unexpected error occured." });
            }
        }

        [HttpPut("update/{fileId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateFileMetadata(Guid fileId, UpdateFileHistoryDTO updateFileHistoryDTO)
        {
            try
            {
                Result result = await fileHistoryUseCases.UpdateFileName(
                    userId, 
                    fileId, 
                    updateFileHistoryDTO
                    );
                if (result.IsFailed)
                {
                    await logger.LogWarning(
                        $"Failed to update file {fileId} metadata.", 
                        result.Errors.Select(e => e.Message).ToList(), 
                        userId
                        );
                    return BadRequest(new { message = result.Errors[0].Message });
                }

                await logger.LogInformation($"File {fileId} metadata updated successfully.", userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                await logger.LogError(ex.Message, ex, userId);
                return StatusCode(500, new { message = "An unexpected error occured." });
            }
        }
    }
}