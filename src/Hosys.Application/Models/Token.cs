namespace Hosys.Application.Models
{
    public class Token
    {
        public required string AccessToken { get; set; }
        public required DateTime ExpireAt { get; set; }
    }
}