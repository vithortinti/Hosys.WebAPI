using System.ComponentModel.DataAnnotations;

namespace Hosys.Application.Data.Outputs.Auth
{
    #nullable disable
    public class AuthSignInDTO
    {
        [Required]
        public string NickName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}