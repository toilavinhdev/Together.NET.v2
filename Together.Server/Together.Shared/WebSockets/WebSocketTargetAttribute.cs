namespace Together.Shared.WebSockets;

public class WebSocketMessageTargetAttribute(string target) : Attribute
{
    public string Target { get; set; } = target;
}