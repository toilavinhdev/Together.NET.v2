namespace Together.Application.Features.FeatureRole.Responses;

public class CreateRoleResponse : TimeTrackingEntity
{
    public string Name { get; set; } = default!;
    
    public string? Description { get; set; }

    public List<string> Claims { get; set; } = default!;
}