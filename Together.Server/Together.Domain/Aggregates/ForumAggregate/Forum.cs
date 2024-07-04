using System.ComponentModel.DataAnnotations.Schema;
using Together.Domain.Abstractions;
using Together.Domain.Aggregates.PostAggregate;
using Together.Domain.Aggregates.TopicAggregate;

namespace Together.Domain.Aggregates.ForumAggregate;

public class Forum : ModifierTrackingEntity, IAggregateRoot
{
    public string Name { get; set; } = default!;
    
    [InverseProperty(nameof(Topic.Forum))]
    public List<Topic>? Topics { get; set; }
    
    [InverseProperty(nameof(Post.Forum))]
    public List<Post>? Posts { get; set; }
}