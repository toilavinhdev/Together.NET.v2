using Together.Shared.Helpers;

namespace Together.Shared.ValueObjects;

public sealed class AppSettings
{
    public string Host { get; set; } = default!;
    
    public PostgresConfig PostgresConfig { get; set; } = default!;
    
    public JwtTokenConfig JwtTokenConfig { get; set; } = default!;
    
    public string RedisConfiguration { get; set; } = default!;

    public GoogleOAuthConfig GoogleOAuthConfig { get; set; } = default!;

    public MailConfig MailConfig { get; set; } = default!;

    public DiscordWebHookConfig DiscordWebHookConfig { get; set; } = default!;
}

public sealed class PostgresConfig
{
    public string ConnectionStrings { get; set; } = default!;
}

public sealed class JwtTokenConfig
{
    public string TokenSigningKey { get; set; } = default!;
    
    public int AccessTokenDurationInMinutes { get; set; }
    
    public int RefreshTokenDurationInDays { get; set; }

    public string? Issuer { get; set; }
    
    public string? Audience { get; set; }
}

public sealed class GoogleOAuthConfig
{
    public string ClientId { get; set; } = default!;
}

public sealed class DiscordWebHookConfig
{
    public ulong WebhookId { get; set; } = default!;
    
    public string WebhookToken { get; set; } = default!;
}