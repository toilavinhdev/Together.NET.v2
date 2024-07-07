namespace Together.Application.Features.FeaturePost.Responses;

public class GetPostResponse : ModifierTrackingEntity
{
    public Guid TopicId { get; set; }
    
    public string TopicName { get; set; } = default!;

    public string? PrefixName { get; set; }
    
    public string? PrefixForeground { get; set; }
    
    public string? PrefixBackground { get; set; }
    
    public string Title { get; set; } = default!;

    public string Body { get; set; } = default!;
    
    public string CreatedByUserName { get; set; } = default!;
    
    public string? CreatedByAvatar { get; set; }
    
    public long ReplyCount { get; set; }
    
    public long VoteUpCount { get; set; }
    
    public long VoteDownCount { get; set; }
    
    public VoteType? Voted { get; set; }
    
    public long ViewCount { get; set; }
}