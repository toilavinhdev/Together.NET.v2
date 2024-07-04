namespace Together.Application.Features.FeatureTopic.Responses;

public class CreateTopicResponse : TimeTrackingEntity
{
    public Guid ForumId { get; set; }
    
    public string Name { get; set; } = default!;
}