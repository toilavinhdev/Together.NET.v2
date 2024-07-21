namespace Together.Application.Features.FeatureUser.Responses;

public class MeResponse : BaseEntity
{
    public string UserName { get; set; } = default!;

    public string Email { get; set; } = default!;
    
    public UserStatus Status { get; set; }
    
    public string? Avatar { get; set; }

    public List<string> Permissions { get; set; } = default!;
}