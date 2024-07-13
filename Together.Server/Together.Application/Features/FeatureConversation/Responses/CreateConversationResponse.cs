namespace Together.Application.Features.FeatureConversation.Responses;

public class CreateConversationResponse : BaseEntity
{
    public string? Name { get; set; }

    public List<Guid> ParticipantIds { get; set; } = default!;
}