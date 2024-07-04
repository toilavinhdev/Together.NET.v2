namespace Together.Application.Features.FeaturePrefix.Responses;

public class CreatePrefixResponse : TimeTrackingEntity
{
    public string Name { get; set; } = default!;

    public string Foreground { get; set; } = default!;

    public string Background { get; set; } = default!;
}