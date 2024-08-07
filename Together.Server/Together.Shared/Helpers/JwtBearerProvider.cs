﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Together.Domain.Aggregates.UserAggregate;
using Together.Shared.ValueObjects;

namespace Together.Shared.Helpers;

public static class JwtBearerProvider
{
    public static string GenerateAccessToken(JwtTokenConfig config, User user)
    {
        return GenerateAccessToken(
            config.TokenSigningKey,
            [
                new Claim("id", user.Id.ToString()),
                new Claim("subId", user.SubId.ToString()),
                new Claim("userName", user.UserName),
                new Claim("email", user.Email),
            ],
            DateTime.UtcNow.AddMinutes(config.AccessTokenDurationInMinutes),
            config.Issuer,
            config.Audience);
    }
    
    public static string GenerateAccessToken(string tokenSingingKey, 
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
    
    public static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
