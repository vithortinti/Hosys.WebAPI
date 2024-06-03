namespace Hosys.Services.Jwt
{
    public class Token
    {
        public required string AccessToken { get; set; }
        public required DateTime ExpireIn { get; set; }
    }
}