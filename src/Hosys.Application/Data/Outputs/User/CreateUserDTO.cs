using Hosys.Identity.Enums;

namespace Hosys.Application.Data.Outputs.Auth
{
    public class CreateUserDTO
    {
        private string? _name;
        public required string Name
        {
            get => _name!;
            set => _name = value.Trim();
        }

        private string? _lastName;
        public required string LastName
        {
            get => _lastName!;
            set => _lastName = value.Trim();
        }

        private string? _nickname;
        public required string NickName
        {
            get => _nickname!;
            set => _nickname = value.Trim();
        }

        private string? _email;
        public required string Email
        {
            get => _email!;
            set => _email = value.Trim();
        }
        
        public required string Password { get; set; }
        public string Role = HosysRoles.USER;
        public DateTime CreatedAt = DateTime.Now;
    }
}