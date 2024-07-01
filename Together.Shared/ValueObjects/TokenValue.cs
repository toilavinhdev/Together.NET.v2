namespace Together.Shared.ValueObjects;

public class TokenValue
{
    public string AccessToken { get; set; } = default!;

    public string RefreshToken { get; set; } = default!;
}