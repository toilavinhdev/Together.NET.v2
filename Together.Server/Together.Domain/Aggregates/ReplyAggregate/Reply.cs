using System.ComponentModel.DataAnnotations.Schema;
using Together.Domain.Abstractions;
using Together.Domain.Aggregates.PostAggregate;
using Together.Domain.Aggregates.UserAggregate;

namespace Together.Domain.Aggregates.ReplyAggregate;

public class Reply : ModifierTrackingEntity, IAggregateRoot
{
    public Guid PostId { get; set; }
    [ForeignKey(nameof(PostId))]
    public Post Post { get; set; } = default!;
    
    public Guid? ParentId { get; set; }
    [ForeignKey(nameof(ParentId))]
    public Reply? Parent { get; set; }
    
    public string Body { get; set; } = default!;
    
    [ForeignKey(nameof(CreatedById))]
    public User CreatedBy { get; set; } = default!;
    
    [InverseProperty(nameof(Parent))]
    public List<Reply>? Children { get; set; }
    
    [InverseProperty(nameof(ReplyVote.Reply))]
    public List<ReplyVote>? ReplyVotes { get; set; }
}