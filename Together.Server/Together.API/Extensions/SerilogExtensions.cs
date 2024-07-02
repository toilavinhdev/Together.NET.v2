using Serilog;

namespace Together.API.Extensions;

public static class SerilogExtensions
{
    public static void SetupSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((_ , configuration) =>
        {
            configuration
                .MinimumLevel.Information()
                .WriteTo.Console();
        });
        
        builder.Logging.ClearProviders();
        
        builder.Logging.AddSerilog();
    }
}