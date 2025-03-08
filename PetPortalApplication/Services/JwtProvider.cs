using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetPortalApplication.AuthConfiguration;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.Models;

namespace PetPortalApplication.Services;

public class JwtProvider (IOptions<JwtOptions> options) : IJwtProvider
{
    private readonly JwtOptions _options = options.Value;
    
    public string GenerateToken(Guid userId, string email, string roleName)
    {
        try
        {
            var claims = new List<Claim>
            {
                new Claim("userId", userId.ToString()),
                new Claim(ClaimTypes.Role, roleName),
                new Claim(ClaimTypes.Email, email)
            };
            
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_options.ExpiresHours),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return token;
        } 
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }
}