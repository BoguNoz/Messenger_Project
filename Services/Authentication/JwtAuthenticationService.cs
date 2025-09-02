using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repositories.UserRepositories;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Services.Authentication;

public class JwtAuthenticationService : IAuthenticationService
{
    #region Dependencies
    private readonly JwtSettings _jwtSettings;
    private readonly IUserRepository _userRepository;
    #endregion Dependencies

    public JwtAuthenticationService(IOptions<JwtSettings> jwtSettings, IUserRepository userRepository)
    {
        _jwtSettings = jwtSettings.Value;
        _userRepository = userRepository;
    }
    
    public async Task<Response<string>> GenerateTokenByUserIdAsync(string userId)
    {
        var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);
        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
            
        var userResponse = await _userRepository.GetByIdAsync(userId);
        if (!userResponse.Status)
        {
            return new Response<string>()
            {
                Object = string.Empty,
            };
        }
        
        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),  
            signingCredentials: credentials
        );
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return new Response<string>
        {
            Object = tokenString,
        };
    }
}