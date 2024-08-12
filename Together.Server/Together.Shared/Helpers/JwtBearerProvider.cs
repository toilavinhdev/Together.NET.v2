using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Together.Domain.Aggregates.UserAggregate;
using Together.Shared.Constants;
using Together.Shared.Extensions;
using Together.Shared.ValueObjects;

namespace Together.Shared.Helpers;

public static class JwtBearerProvider
{
    public static string GenerateAccessToken(JwtTokenConfig config, IdentityPrivilege privilege)
    {
        return GenerateAccessToken(config, privilege.Id, privilege.SubId, privilege.UserName, privilege.Email);
    }
    
    public static string GenerateAccessToken(JwtTokenConfig config, User user)
    {
        return GenerateAccessToken(config, user.Id, user.SubId, user.UserName, user.Email);
    }
    
    private static string GenerateAccessToken(JwtTokenConfig config, 
        Guid id, 
        long subId, 
        string userName, 
        string email)
    {
        return GenerateAccessToken(
            config.TokenSigningKey,
            [
                new Claim("id", id.ToString()),
                new Claim("subId", subId.ToString()),
                new Claim("userName", userName),
                new Claim("email", email),
            ],
            DateTime.UtcNow.AddMinutes(config.AccessTokenDurationInMinutes),
            config.Issuer,
            config.Audience);
    }
    
    private static string GenerateAccessToken(string tokenSingingKey, 
        IEnumerable<Claim> claimsPrincipal, 
        DateTime? expireAt = null, 
        string? issuer = null, 
        string? audience = null)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSingingKey));
        
        var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = issuer,
            Audience = audience,
            IssuedAt = DateTime.Now,
            Subject = new ClaimsIdentity(claimsPrincipal),
            Expires = expireAt,
            SigningCredentials = credential
        };
        
        var handler = new JwtSecurityTokenHandler();
        
        return handler.WriteToken(handler.CreateToken(descriptor));
    }
    
    public static CurrentUserClaims DecodeAccessToken(string accessToken)
    {
        var decodedToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
        
        var id = decodedToken.Claims.FirstOrDefault(x => x.Type.Equals("id"))?.Value.ToGuid() 
                 ?? throw new NullReferenceException("Claim type 'id' is required");
        var subId = decodedToken.Claims.FirstOrDefault(x => x.Type.Equals("subId"))?.Value.ToLong()
                    ?? throw new NullReferenceException("Claim type 'subId' is required");
        var userName = decodedToken.Claims.FirstOrDefault(x => x.Type.Equals("userName"))?.Value
                       ?? throw new NullReferenceException("Claim type 'userName' is required");
        var email = decodedToken.Claims.FirstOrDefault(x => x.Type.Equals("email"))?.Value
                    ?? throw new NullReferenceException("Claim type 'email' is required");
        
        if (!RegexPatterns.EmailRegex.IsMatch(email)) throw new InvalidCastException("Email invalid");
        
        return new CurrentUserClaims
        {
            Id = id,
            SubId = subId,
            UserName = userName,
            Email = email
        };
    }
    
    public static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
