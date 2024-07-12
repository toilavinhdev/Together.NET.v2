namespace Together.Application.Features.FeatureMessage.Responses;

public class ListMessageResponse : PaginationResult<MessageViewModel>
{
    public Dictionary<string, object>? Extra { get; set; }
}

public class MessageViewModel : ModifierTrackingEntity
{
    public Guid ConversationId { get; set; }
    
    public string Text { get; set; } = default!;
    
    public string CreatedByUserName { get; set; } = default!;
    
    public string? CreatedByAvatar { get; set; }
}