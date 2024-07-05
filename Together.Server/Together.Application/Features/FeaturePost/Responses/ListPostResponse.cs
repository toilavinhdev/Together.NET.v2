namespace Together.Application.Features.FeaturePost.Responses;

public class ListPostResponse : PaginationResult<PostViewModel>
{
    public object? Extra { get; set; }
}

public class PostViewModel : ModifierTrackingEntity
{
    public Guid ForumId { get; set; }
    
    public Guid TopicId { get; set; }

    public string TopicName { get; set; } = default!;
    
    public Guid? PrefixId { get; set; }

    public string? PrefixName { get; set; } = default!;
    
    public string? PrefixForeground { get; set; } = default!;
    
    public string? PrefixBackground { get; set; } = default!;
    
    public string Title { get; set; } = default!;

    public string Body { get; set; } = default!;

    public string CreatedByUserName { get; set; } = default!;
    
    public string? CreatedByAvatar { get; set; }
    
    public long ReplyCount { get; set; }
}