using System.ComponentModel.DataAnnotations.Schema;
using Together.Domain.Abstractions;

namespace Together.Domain.Aggregates.PostAggregate;

public class Prefix : ModifierTrackingEntity
{
    public string Name { get; set; } = default!;

    public string Foreground { get; set; } = default!;

    public string Background { get; set; } = default!;
    
    [InverseProperty(nameof(Post.Prefix))]
    public List<Post>? Posts { get; set; }
}