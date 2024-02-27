namespace Hosys.Application.Data.Outputs.User
{
    public class ReadUserDTO
    {
        public required int PublicId { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string NickName { get; set; }
        public required string Email { get; set; }
        public required DateTime CreatedAt { get; set; }
    }
}