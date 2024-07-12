namespace Together.Application.Features.FeatureMessage.Responses;

public class SendMessageResponse : TimeTrackingEntity
{
    public Guid ConversationId { get; set; }

    public string Text { get; set; } = default!;
}