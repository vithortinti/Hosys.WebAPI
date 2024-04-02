using Microsoft.AspNetCore.Http;

namespace Hosys.Application.Ports
{
    public class FileInput
    {
        public required IFormFile File { get; set; }
    }
}