using Together.Domain.Abstractions;

namespace Together.Domain.Aggregates.FileAggregate;

public class File : ModifierTrackingEntity, IAggregateRoot
{
    public string PublicId { get; set; } = default!;
    
    public string DisplayName { get; set; } = default!;
    
    public string OriginalName { get; set; } = default!;
    
    public string Url { get; set; } = default!;

    public string Format { get; set; } = default!;
}