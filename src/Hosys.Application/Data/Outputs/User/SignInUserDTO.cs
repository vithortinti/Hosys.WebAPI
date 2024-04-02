using System.ComponentModel.DataAnnotations;

namespace Hosys.Application.Data.Outputs.User
{
    #nullable disable
    public class SignInUserDTO
    {
        [Required]
        public string NickName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}