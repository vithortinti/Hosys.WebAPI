namespace Hosys.Application.Data.Outputs.Auth
{
    public class AuthTokenDTO
    {
        public required string AccessToken { get; set; }
        public required DateTime ExpireIn { get; set; }
    }
}