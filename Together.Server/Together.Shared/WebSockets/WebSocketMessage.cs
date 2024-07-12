namespace Together.Shared.WebSockets;

public class WebSocketMessage
{
    public string Target { get; set; } = default!;

    public object? Message { get; set; }
}

public class WebSocketMessage<T>
{
    public string Target { get; set; } = default!;

    public T? Message { get; set; }
}