using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Together.Application.Authorization;
using Together.Shared.ValueObjects;

namespace Together.API.Extensions;

public static class AuthExtensions
{
    public static void AddCoreAuth(this IServiceCollection services, JwtTokenConfig config)
    {
        services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(
                o =>
                {
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.TokenSigningKey));

                    o.TokenValidationParameters.IssuerSigningKey = key;
                    o.TokenValidationParameters.ValidateIssuerSigningKey = true;
                    o.TokenValidationParameters.ValidateLifetime = true;
                    o.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
                    o.TokenValidationParameters.ValidAudience = config.Audience;
                    o.TokenValidationParameters.ValidateAudience = config.Audience is not null;
                    o.TokenValidationParameters.ValidIssuer = config.Issuer;
                    o.TokenValidationParameters.ValidateIssuer = config.Audience is not null;
                });
        
        services.AddSingleton<IAuthorizationPolicyProvider, TogetherPolicyProvider>();
        services.AddSingleton<IAuthorizationHandler, TogetherAuthorizationHandler>();
        services.AddSingleton<IAuthorizationMiddlewareResultHandler, TogetherAuthorizationMiddlewareResultHandler>();

        
        services.AddAuthorization();
    }

    public static void UseCoreAuth(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}