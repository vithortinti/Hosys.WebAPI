namespace Hosys.Application.Data.Outputs.Auth
{
    public class ReadUserDTO
    {
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required int PublicId { get; set; }
        public required string NickName { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required bool Active { get; set; }
    }
}