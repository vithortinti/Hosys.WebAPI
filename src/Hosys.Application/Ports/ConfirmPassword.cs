using System.ComponentModel.DataAnnotations;

namespace Hosys.Application.Ports
{
    public class ConfirmPassword
    {
        public required string Password { get; set; }
    }
}