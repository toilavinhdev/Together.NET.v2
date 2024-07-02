using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Together.Shared.Constants;
using Together.Shared.Extensions;

namespace Together.Shared.ValueObjects;

public sealed class CurrentUserClaims
{
    public Guid Id { get; set; }
    
    public long SubId { get; set; }

    public string UserName { get; set; } = default!;

    public string Email { get; set; } = default!;
    
    public static CurrentUserClaims GetValue(IHttpContextAccessor httpContextAccessor)
    {
        var accessToken = httpContextAccessor.HttpContext?.Request.Headers
            .FirstOrDefault(x => x.Key.Equals("Authorization"))
            .Value
            .ToString()
            .Split(" ")
            .LastOrDefault();

        if (string.IsNullOrEmpty(accessToken) ) return default!;
        
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
}