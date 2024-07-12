namespace Together.Application.Sockets.WebSocketMessages;

[WebSocketMessageTarget(nameof(WebSocketServerTarget.Ping))]
public class PingWebSocketMessage;