namespace Together.Shared.ValueObjects;

public sealed class AppSettings
{
    public PostgresConfig PostgresConfig { get; set; } = default!;
    
    public JwtTokenConfig JwtTokenConfig { get; set; } = default!;
    
    public string RedisConfiguration { get; set; } = default!;
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