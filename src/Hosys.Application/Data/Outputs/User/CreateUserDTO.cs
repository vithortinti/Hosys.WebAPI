namespace Hosys.Application.Data.Outputs.User
{
    public class CreateUserDTO
    {
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string Nickname { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string Role = "USER";
        public DateTime CreatedAt = DateTime.Now;
    }
}