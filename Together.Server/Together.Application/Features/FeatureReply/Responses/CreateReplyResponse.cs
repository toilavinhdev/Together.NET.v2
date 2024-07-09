namespace Together.Application.Features.FeatureReply.Responses;

public class CreateReplyResponse : TimeTrackingEntity
{
    public Guid PostId { get; set; }
    
    public Guid? ParentId { get; set; }
    
    public string Body { get; set; } = default!;
}