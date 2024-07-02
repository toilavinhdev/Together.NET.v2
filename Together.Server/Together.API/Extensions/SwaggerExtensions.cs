using Microsoft.OpenApi.Models;

namespace Together.API.Extensions;

public static class SwaggerExtensions
{
    public static void AddSwaggerDocument(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(
            o =>
            {
                o.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = Metadata.Name,
                    Version = "v1",
                    Description = "Summary of APIs of Together.NET forum"
                });
                
                o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. " +
                                  "\r\n\r\n Enter 'Bearer' [space] and then your token in the text input below." +
                                  "\r\n\r\nExample: \"Bearer accessToken\"",
                });
                
                o.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        }, []
                    }
                });
            });
    }
    
    public static void UseSwaggerDocument(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(o =>
        {
            o.DocumentTitle = Metadata.Name;
        });
    }
}