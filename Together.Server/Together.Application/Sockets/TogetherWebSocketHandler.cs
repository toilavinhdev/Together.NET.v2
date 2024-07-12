using System.Net.WebSockets;

namespace Together.Application.Sockets;

public sealed class TogetherWebSocketHandler(ConnectionManager connectionManager) : WebSocketHandler(connectionManager)
{
    protected override Task ReceiveAsync(string socketId, WebSocket socket, WebSocketMessage message)
    {
        switch (message.Target)
        {
            case WebSocketServerTarget.Ping:
                break;
        }
        
        Console.WriteLine(111111111111111 + message.ToJson());

        return Task.CompletedTask;
    }
}