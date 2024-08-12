using System.Net.WebSockets;

namespace Together.Application.Sockets;

public sealed class TogetherWebSocketHandler(ConnectionManager connectionManager, IRedisService redisService) 
    : WebSocketHandler(connectionManager)
{
    public override async Task OnConnectedAsync(string id, WebSocket socket)
    {
        await base.OnConnectedAsync(id, socket);
        await redisService.SetAddAsync(TogetherRedisKeys.OnlineUserKey(), id);
    }

    public override async Task OnDisconnectedAsync(string id, WebSocket socket)
    {
        await base.OnDisconnectedAsync(id, socket);
        await redisService.SetRemoveAsync(TogetherRedisKeys.OnlineUserKey(), id);
    }

    protected override Task ReceiveAsync(string socketId, WebSocket socket, WebSocketMessage message)
    {
        switch (message.Target)
        {
            case WebSocketServerTarget.Ping:
                break;
        }
        
        return Task.CompletedTask;
    }
}