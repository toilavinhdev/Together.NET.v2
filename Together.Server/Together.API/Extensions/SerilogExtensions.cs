using Serilog;
using Serilog.Events;
using Serilog.Sinks.Discord;
using Together.Shared.ValueObjects;

namespace Together.API.Extensions;

public static class SerilogExtensions
{
    public static void SetupSerilog(this WebApplicationBuilder builder, AppSettings appSettings)
    {
        builder.Host.UseSerilog((_ , configuration) =>
        {
            configuration
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Async(cfg =>
                {
                    cfg.Discord(
                        webhookId: appSettings.DiscordWebHookConfig.WebhookId, 
                        webhookToken: appSettings.DiscordWebHookConfig.WebhookToken,
                        restrictedToMinimumLevel: LogEventLevel.Warning);
                });
        });
        
        builder.Logging.ClearProviders();
        
        builder.Logging.AddSerilog();
    }
}