namespace Together.API.Extensions;

public static class CorsExtensions
{
    public static void AddCoreCors(this IServiceCollection services)
    {
        services.AddCors(o =>
        {
            o.AddPolicy(Metadata.Name, b =>
            {
                b.AllowCredentials().AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(_ => true);
            });
        });
    }

    public static void UseCoreCors(this WebApplication app)
    {
        app.UseCors(Metadata.Name);
    }
}