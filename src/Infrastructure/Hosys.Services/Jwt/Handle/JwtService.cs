using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Hosys.Domain.Models.User;
using Microsoft.IdentityModel.Tokens;

namespace Hosys.Services.Jwt.Handle
{
    public class JwtService(string secret, string issuer, string audience, int expireIn)
    {
        private readonly string _secret = secret;
        private readonly string _issuer = issuer;
        private readonly string _audience = audience;
        private readonly int _expireIn = expireIn;

        public Token GenerateToken(User user)
        {
            // Create symmetric security key
            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_secret));

            // Create the fingerprint
            SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);

            // Insert the claims
            var indentity = new ClaimsIdentity();
            indentity.AddClaims([
                new Claim("id", user.Id.ToString()),
                new Claim("firstName", user.Name),
                new Claim("lastName", user.LastName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            ]);

            // Create the token
            JwtSecurityTokenHandler jwtHandler = new();
            var token = jwtHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _issuer,
                Audience = _audience,
                Subject = indentity,
                Expires = DateTime.UtcNow.AddMinutes(_expireIn),
                IssuedAt = DateTime.UtcNow,
                SigningCredentials = credentials
            });

            return new Token
            {
                AccessToken = jwtHandler.WriteToken(token),
                ExpireIn = token.ValidTo
            };
        }
    }
}