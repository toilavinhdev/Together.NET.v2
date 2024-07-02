using Together.Shared.ValueObjects;

namespace Together.API.Extensions;

public static class EnvironmentExtensions
{
    public static void SetupEnvironment(this WebApplicationBuilder builder, 
        out AppSettings appSettings)
    {
        appSettings = new AppSettings();
        var environment = builder.Environment;
        var json = $"appsettings.{environment.EnvironmentName}.json";
        var path = Path.Combine("AppSettings", json);
        
        new ConfigurationBuilder()
            .SetBasePath(environment.ContentRootPath)
            .AddJsonFile(path)
            .AddEnvironmentVariables()
            .Build()
            .Bind(appSettings);
    }
}