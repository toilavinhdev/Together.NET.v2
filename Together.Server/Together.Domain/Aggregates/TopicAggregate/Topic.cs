using System.ComponentModel.DataAnnotations.Schema;
using Together.Domain.Abstractions;
using Together.Domain.Aggregates.ForumAggregate;
using Together.Domain.Aggregates.PostAggregate;

namespace Together.Domain.Aggregates.TopicAggregate;

public class Topic : ModifierTrackingEntity, IAggregateRoot
{
    public Guid ForumId { get; set; }
    [ForeignKey(nameof(ForumId))]
    public Forum Forum { get; set; } = default!;

    public string Name { get; set; } = default!;
    
    public string? Description { get; set; }
    
    [InverseProperty(nameof(Post.Topic))]
    public List<Post>? Posts { get; set; }
}