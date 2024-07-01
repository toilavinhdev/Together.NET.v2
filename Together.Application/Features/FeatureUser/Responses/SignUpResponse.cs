namespace Together.Application.Features.FeatureUser.Responses;

public class SignUpResponse : TimeTrackingEntity
{
    public string UserName { get; set; } = default!;

    public string Email { get; set; } = default!;
    
    public UserStatus Status { get; set; }
    
    public Gender Gender { get; set; }
    
    public string? FullName { get; set; }
    
    public string? Avatar { get; set; }
}