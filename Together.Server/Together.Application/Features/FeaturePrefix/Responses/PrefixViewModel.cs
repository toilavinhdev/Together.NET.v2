namespace Together.Application.Features.FeaturePrefix.Responses;

public class PrefixViewModel : TimeTrackingEntity
{
    public string Name { get; set; } = default!;

    public string Foreground { get; set; } = default!;

    public string Background { get; set; } = default!;
}