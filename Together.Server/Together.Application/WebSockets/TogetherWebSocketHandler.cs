using System.Net.WebSockets;

namespace Together.Application.WebSockets;

public sealed class TogetherWebSocketHandler(ConnectionManager connectionManager) : WebSocketHandler(connectionManager)
{
    protected override Task ReceiveAsync(string socketId, WebSocket socket, WebSocketMessage message)
    {
        switch (message.Target)
        {
            case WebSocketTarget.Ping:
                break;
        }

        return Task.CompletedTask;
    }
}