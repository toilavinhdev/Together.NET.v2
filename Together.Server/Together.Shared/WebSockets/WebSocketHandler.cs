using System.Net.WebSockets;
using System.Text;
using Together.Shared.Extensions;

namespace Together.Shared.WebSockets;

public abstract class WebSocketHandler(ConnectionManager connectionManager)
{
    public ConnectionManager ConnectionManager { get; set; } = connectionManager;
    
    protected abstract Task ReceiveAsync(string socketId, WebSocket socket, WebSocketMessage message);

    public async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
    {
        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

        var socketId = ConnectionManager.GetId(socket);

        if (string.IsNullOrEmpty(socketId)) return;

        await ReceiveAsync(socketId, socket, message.ToObject<WebSocketMessage>());
    }

    public virtual Task OnConnected(string id, WebSocket socket)
    {
        ConnectionManager.AddSocket(id, socket);
        return Task.CompletedTask;
    }

    public virtual async Task OnDisconnected(WebSocket socket)
    {
        await ConnectionManager.RemoveSocketAsync(socket);
    }
    
    private static async Task SendMessageAsync(WebSocket socket, string message)
    {
        if (socket.State != WebSocketState.Open)
            return;

        await socket.SendAsync(
            buffer: new ArraySegment<byte>(
                array: Encoding.UTF8.GetBytes(message),
                offset: 0,
                count: message.Length),
            messageType: WebSocketMessageType.Text,
            endOfMessage: true,
            cancellationToken: CancellationToken.None);
    }
    
    public async Task SendMessageAsync(string socketId, string message)
    {
        var sockets = ConnectionManager.GetSockets(socketId);
        if (sockets is null) return;

        foreach (var socket in sockets)
        {
            await SendMessageAsync(socket, message);
        }
    }
    
    public async Task SendMessageToAllAsync(string message)
    {
        foreach (var pair in ConnectionManager.GetAll())
        {
            foreach (var socket in pair.Value.Where(socket => socket.State == WebSocketState.Open))
            {
                await SendMessageAsync(socket, message);
            }
        }
    }
}