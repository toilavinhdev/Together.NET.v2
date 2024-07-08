namespace Together.Application.Features.FeatureUser.Responses;

public class GetUserResponse : TimeTrackingEntity
{
    public string UserName { get; set; } = default!;
    
    public Gender Gender { get; set; }

    public string? FullName { get; set; }
    
    public string? Avatar { get; set; }
    
    public string? Biography { get; set; }
    
    public long PostCount { get; set; }
    
    public long ReplyCount { get; set; }
}