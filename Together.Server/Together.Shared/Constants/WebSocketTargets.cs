namespace Together.Shared.Constants;

public static class WebSocketServerTarget
{
    public const string Ping = nameof(Ping);
}

public static class WebSocketClientTarget
{
    public const string ReceivedNotification = nameof(ReceivedNotification);
    
    public const string ReceivedMessage = nameof(ReceivedMessage);
}