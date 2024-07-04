namespace Together.Application.Features.FeatureForum.Responses;

public class CreateForumResponse : TimeTrackingEntity
{
    public string Name { get; set; } = default!;
}