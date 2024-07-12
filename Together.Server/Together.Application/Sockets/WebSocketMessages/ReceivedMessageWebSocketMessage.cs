namespace Together.Application.Sockets.WebSocketMessages;

[WebSocketMessageTarget(nameof(WebSocketClientTarget.ReceivedMessage))]
public class ReceivedMessageWebSocketMessage : ModifierTrackingEntity
{
    public Guid ConversationId { get; set; }
    
    public string Text { get; set; } = default!;

    public string CreatedByUserName { get; set; } = default!;

    public string? CreatedByAvatar { get; set; }
}